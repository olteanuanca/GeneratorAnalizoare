using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using System.CodeDom;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.Diagnostics;

namespace ALL1
{
  
    class CodeGenerator
    {
        public Grammar G = Grammar.Instance();

        CodeCompileUnit targetUnit;
        CodeTypeDeclaration targetClass;
        private const string outputFileName = "SampleCode.cs";
        public CodeMemberMethod[] N;

        public CodeGenerator()
        {
            targetUnit = new CodeCompileUnit();
            CodeNamespace samples = new CodeNamespace("ALL12");

            samples.Imports.Add(new CodeNamespaceImport("System"));
            samples.Imports.Add(new CodeNamespaceImport("System.IO"));

            targetClass = new CodeTypeDeclaration("Verify");
            targetClass.IsClass = true;
            targetClass.TypeAttributes =TypeAttributes.Public;
            
            samples.Types.Add(targetClass);
            targetUnit.Namespaces.Add(samples);

        }

        public void AddFields()
        {

         N = new CodeMemberMethod[G.nontermNb];

        // Declare the Sentence field.
        CodeMemberField SentenceField = new CodeMemberField();
            SentenceField.Attributes = MemberAttributes.Public | MemberAttributes.Static;
            SentenceField.Name = "Sentence";
            SentenceField.Type = new CodeTypeReference("System.string", 1);
            SentenceField.InitExpression = new CodeArrayCreateExpression("System.String", 10);
            SentenceField.Comments.Add(new CodeCommentStatement("Propozitia:"));
            targetClass.Members.Add(SentenceField);

            
            // Declare the Position field
            CodeMemberField PositionField = new CodeMemberField();
            PositionField.Name = "Position";
            PositionField.Type = new CodeTypeReference(typeof(Int32), (System.CodeDom.CodeTypeReferenceOptions)0);
            PositionField.Attributes = MemberAttributes.Public | MemberAttributes.Static;
            PositionField.InitExpression = new CodePrimitiveExpression(0);
            targetClass.Members.Add(PositionField);
            PositionField.Comments.Add(new CodeCommentStatement("Pozitia in vector:"));

             }
        public void AddEntryPoint()
        {
            CodeEntryPointMethod start = new CodeEntryPointMethod();

            targetClass.Members.Add(start);

            CodeSnippetExpression exp2 = new CodeSnippetExpression("Verify V=new Verify()");
            CodeExpressionStatement stm2 = new CodeExpressionStatement(exp2);
            start.Statements.Add(stm2);

            CodeSnippetExpression exp1 = new CodeSnippetExpression("V.ReadSentence()");
            CodeExpressionStatement stm1 = new CodeExpressionStatement(exp1);
            start.Statements.Add(stm1);

            CodeSnippetExpression exp5 = new CodeSnippetExpression("try{");
            CodeExpressionStatement stm5 = new CodeExpressionStatement(exp5);
            start.Statements.Add(stm5);

            CodeSnippetExpression exp7 = new CodeSnippetExpression($"{G.startSymb}()");
            CodeExpressionStatement stm7 = new CodeExpressionStatement(exp7);
            start.Statements.Add(stm7);

            CodeSnippetExpression exp3 = new CodeSnippetExpression("if(Sentence[Position]==\"$\") \n System.Console.WriteLine(\"Corect\")") ;
            CodeExpressionStatement stm3 = new CodeExpressionStatement(exp3);
           start.Statements.Add(stm3);

            CodeSnippetExpression exp4 = new CodeSnippetExpression("else \n \t\t\t\t\t System.Console.WriteLine(\"Incorect\")");
            CodeExpressionStatement stm4 = new CodeExpressionStatement(exp4);
            start.Statements.Add(stm4);


            CodeSnippetExpression exp6 = new CodeSnippetExpression("} \n \t\t\t catch(Exception E){ \n \t\t\t\t\t\t System.Console.WriteLine(E.Message); }\n  \t\t\t\t\t System.Console.Read() ");
            CodeExpressionStatement stm6 = new CodeExpressionStatement(exp6);
            start.Statements.Add(stm6);
        }

        public void AddMethods()
        {
           
            for (int i = 0; i < G.nontermNb; i++)
            {
                N[i] = new CodeMemberMethod();
                N[i].Name = G.nonterminal[i];
                N[i].Attributes = MemberAttributes.Static;
                targetClass.Members.Add(N[i]);
            }
                       
           
            for (int i = 0; i < G.nontermNb; i++)
            {
                CodeSnippetExpression exp9 = new CodeSnippetExpression("int check=0");
                CodeExpressionStatement stm9 = new CodeExpressionStatement(exp9);
                N[i].Statements.Add(stm9);

                for (int j = 0; j < G.tableNb; j++)
                {
                    if (N[i].Name == G.t[j].nonterminal)
                    {

                        
                        ///term
                        CodeSnippetExpression exp1 = new CodeSnippetExpression($"if(Sentence[Position]==\"{G.t[j].terminal}\")"); 
                        CodeSnippetExpression exp5 = new CodeSnippetExpression("{ \n \t\t\t\t check=1");
                        exp1.Value += exp5.Value;
                        CodeExpressionStatement stm5 = new CodeExpressionStatement(exp1);
                        N[i].Statements.Add(stm5);



                        for (int k = 1; k < G.ruleSize[G.t[j].rule]; k++)
                        {
                            if (G.checkifEx(G.rule[G.t[j].rule, k]) == true)
                            {
                                

                                CodeSnippetExpression exp8 = new CodeSnippetExpression($"{G.rule[G.t[j].rule, k]}()");
                                CodeExpressionStatement stm8 = new CodeExpressionStatement(exp8);
                                N[i].Statements.Add(stm8);

                            }
                            else if (G.isTerminal(G.rule[G.t[j].rule, k]) == true)
                            {

                                CodeSnippetExpression exp7 = new CodeSnippetExpression($"if (Sentence[Position] ==\"{G.rule[G.t[j].rule,k]}\") \n \t\t\t\t\t Position++");
                                CodeExpressionStatement stm7 = new CodeExpressionStatement(exp7);
                                N[i].Statements.Add(stm7);

                         
                               
                            }
                        }
                        CodeSnippetExpression exp2 = new CodeSnippetExpression("}");
                        CodeExpressionStatement stm2 = new CodeExpressionStatement(exp2);
                        N[i].Statements.Add(stm2);

                       

                    }

                }
                CodeSnippetExpression exp10 = new CodeSnippetExpression("if(check==0) throw new Exception(\"Another symbols expected at position\")");
                CodeExpressionStatement stm10 = new CodeExpressionStatement(exp10);
                N[i].Statements.Add(stm10);

            }

            

        }
        public void InitializeVars()
        {

            CodeMemberMethod f1 = new CodeMemberMethod();
            //Assign a name for the method.
            f1.Name = "ReadSentence";
            f1.Attributes = MemberAttributes.Public;


            CodeSnippetExpression exp1 = new CodeSnippetExpression("String tempSent = System.Console.ReadLine()");
            CodeExpressionStatement stm1 = new CodeExpressionStatement(exp1);
            f1.Statements.Add(stm1);


            CodeSnippetExpression exp5 = new CodeSnippetExpression("Sentence=tempSent.Split(' ')");
            CodeExpressionStatement stm5 = new CodeExpressionStatement(exp5);
            f1.Statements.Add(stm5);


            CodeSnippetExpression exp4 = new CodeSnippetExpression("System.Console.ReadKey()");
            CodeExpressionStatement stm4 = new CodeExpressionStatement(exp4);
            f1.Statements.Add(stm4);

            targetClass.Members.Add(f1);

        }

        public void GenerateCSharpCode(string fileName)
        {
            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
            CodeGeneratorOptions options = new CodeGeneratorOptions();
            options.BracingStyle = "C";
            using (StreamWriter sourceWriter = new StreamWriter(fileName))
            {
                provider.GenerateCodeFromCompileUnit(
                    targetUnit, sourceWriter, options);
            }
        }

        public bool CompileCSharpCode(string sourceFile, string exeFile)
        {

            CSharpCodeProvider provider = new CSharpCodeProvider();

            // Build the parameters for source compilation.
            CompilerParameters cp = new CompilerParameters();

            // Add an assembly reference.
            cp.ReferencedAssemblies.Add("System.dll");

            // Generate an executable instead of
            // a class library.
            cp.GenerateExecutable = true;

            // Set the assembly file name to generate.
            cp.OutputAssembly = exeFile;

            // Save the assembly as a physical file.
            cp.GenerateInMemory = false;

            // Invoke compilation.
            CompilerResults cr = provider.CompileAssemblyFromFile(cp, sourceFile);

            if (cr.Errors.Count > 0)
            {
                // Display compilation errors.
                Console.WriteLine("Errors building {0} into {1}",
                    sourceFile, cr.PathToAssembly);
                foreach (CompilerError ce in cr.Errors)
                {
                    Console.WriteLine("  {0}", ce.ToString());
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("Source {0} built into {1} successfully.",
                    sourceFile, cr.PathToAssembly);
            }

            // Return the results of compilation.
            if (cr.Errors.Count > 0)
            {
                return false;
            }
            else
            {
                Process.Start("2.exe");

                return true;
            }
        }




    }
}
