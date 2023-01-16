using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Security.AccessControl;

namespace TestDataGridView {
    public class ClassGenerator {
        public static Type GenerateType(string[] keys, string name) {
            var aName = new AssemblyName("ClassGenerator");
            var assBuilder = AssemblyBuilder.DefineDynamicAssembly(aName, AssemblyBuilderAccess.Run);
            var assModule = assBuilder.DefineDynamicModule(aName.Name!);

            var newClass = assModule.DefineType(name, 
                TypeAttributes.Public | 
                TypeAttributes.Class |
                TypeAttributes.AutoClass |
                TypeAttributes.AnsiClass |
                TypeAttributes.BeforeFieldInit |
                TypeAttributes.AutoLayout, null);
            ConstructorBuilder ctor0 = newClass.DefineDefaultConstructor(MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName);
            
            foreach (var k in keys) {
                Type type = typeof(string);
                FieldBuilder prop = newClass.DefineField(k, type, FieldAttributes.Private);
                PropertyBuilder propPublic = newClass.DefineProperty($"P_{k}", PropertyAttributes.HasDefault, type, null);
                
                MethodAttributes getSetAttr = MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig;
                
                MethodBuilder propGetAccessor = newClass.DefineMethod($"get_{k}", getSetAttr, type, null);
                ILGenerator propGetIL = propGetAccessor.GetILGenerator();
                propGetIL.Emit(OpCodes.Ldarg_0);
                propGetIL.Emit(OpCodes.Ldfld, prop);
                propGetIL.Emit(OpCodes.Ret);
                
                MethodBuilder propSetAccessor = newClass.DefineMethod($"set_{k}", getSetAttr, null,  new Type[]{type});
                ILGenerator propSetIL = propSetAccessor.GetILGenerator();
                propSetIL.Emit(OpCodes.Ldarg_0);
                propSetIL.Emit(OpCodes.Ldarg_1);
                propSetIL.Emit(OpCodes.Stfld, prop);
                propSetIL.Emit(OpCodes.Ret);
                
                propPublic.SetGetMethod(propGetAccessor);
                propPublic.SetSetMethod(propSetAccessor);
            }

            //var t = newClass.CreateType();
            // var result = new List<object?>();
            // foreach (var d in data) {
            //     object? o = Activator.CreateInstance(t);
            //     foreach (var dic in data) {
            //         foreach (var dd in dic) {
            //             PropertyInfo pi = t.GetProperty($"P_{dd.Key}");
            //             pi.SetValue(o, dd.Value);
            //         }
            //     }
            //     result.Add(o);
            // }
            //return result;
            return newClass.CreateType();
        }

        public static dynamic GetObject(Type myType, Dictionary<string, string> data) {
            object? result = Activator.CreateInstance(myType);
            foreach (var dic in data) {
                    PropertyInfo pi = myType.GetProperty($"P_{dic.Key}");
                    pi.SetValue(result, dic.Value);
            }
            return result;
        }
    }
}