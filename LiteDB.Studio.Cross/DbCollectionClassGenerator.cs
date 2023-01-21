using LiteDB.Studio.Cross.Models;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace LiteDB.Studio.Cross {
    public class DbCollectionClassGenerator {
        public static Type GenerateCollectionClass(IEnumerable<PropertyModel> props, string name) {
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
            
            foreach (var prp in props) {
                Type type = prp.Type;
                FieldBuilder prop = newClass.DefineField('_'+prp.Name, type, FieldAttributes.Private);
                PropertyBuilder propPublic = newClass.DefineProperty(prp.Name, PropertyAttributes.HasDefault, type, null);
                
                MethodAttributes getSetAttr = MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig;
                
                MethodBuilder propGetAccessor = newClass.DefineMethod($"get_{prp}", getSetAttr, type, null);
                ILGenerator propGetIL = propGetAccessor.GetILGenerator();
                propGetIL.Emit(OpCodes.Ldarg_0);
                propGetIL.Emit(OpCodes.Ldfld, prop);
                propGetIL.Emit(OpCodes.Ret);
                
                MethodBuilder propSetAccessor = newClass.DefineMethod($"set_{prp}", getSetAttr, null,  new Type[]{type});
                ILGenerator propSetIL = propSetAccessor.GetILGenerator();
                propSetIL.Emit(OpCodes.Ldarg_0);
                propSetIL.Emit(OpCodes.Ldarg_1);
                propSetIL.Emit(OpCodes.Stfld, prop);
                propSetIL.Emit(OpCodes.Ret);
                
                propPublic.SetGetMethod(propGetAccessor);
                propPublic.SetSetMethod(propSetAccessor);
            }

            return newClass.CreateType();
        }
        
        public static dynamic? GetObject(Type myType, Dictionary<string, dynamic> data) {
            object? result = Activator.CreateInstance(myType);
            foreach (var dic in data) {
                PropertyInfo? pi = myType.GetProperty(dic.Key);
                if (pi != null) pi.SetValue(result, dic.Value);
            }
            return result;
        }

    }
}