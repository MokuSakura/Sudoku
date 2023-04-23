// using System.Reflection;
// using log4net;
// using MokuSakura.Sudoku.Requirement;
//
// namespace MokuSakura.Sudoku.Starter;
//
// public class SudokuStarterReflectionUtils
// {
//     public IDictionary<String, ISudokuStarter> SudokuStarters { get; init; } = new Dictionary<String, ISudokuStarter>();
//     private static ILog Log => LogManager.GetLogger(typeof(SudokuRequirementReflectionUtils));
//
//     public static SudokuStarterReflectionUtils Instance => new();
//
//     private SudokuStarterReflectionUtils()
//     {
//         Type starterType = typeof(ISudokuStarter);
//         foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
//         {
//             foreach (Type type in assembly.GetTypes())
//             {
//                 if (!starterType.IsAssignableFrom(type))
//                 {
//                     continue;
//                 }
//
//                 if (starterType == type)
//                 {
//                     continue;
//                 }
//
//                 ConstructorInfo? constructorInfo = type.GetConstructor(Array.Empty<Type>());
//                 if (constructorInfo == null)
//                 {
//                     Log.Warn($"{type.FullName} does not contain a parameterless constructor. Cannot initialize it.");
//                     continue;
//                 }
//
//                 ISudokuStarter starter = (ISudokuStarter)constructorInfo.Invoke(Array.Empty<Object>());
//                 foreach (String s in starter.RegisterName)
//                 {
//                     SudokuStarters[s] = starter;
//                 }
//             }
//         }
//     }
// }