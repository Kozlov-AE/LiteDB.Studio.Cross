// This code creates an assembly that contains one type,
// named "MyDynamicType", that has a private field, a property
// that gets and sets the private field, constructors that
// initialize the private field, and a method that multiplies
// a user-supplied number by the private field value and returns
// the result. In C# the type might look like this:
/*
public class MyDynamicType
{
    private int m_number;

    public MyDynamicType() : this(42) {}
    public MyDynamicType(int initNumber)
    {
        m_number = initNumber;
    }

    public int Number
    {
        get { return m_number; }
        set { m_number = value; }
    }

    public int MyMethod(int multiplier)
    {
        return m_number * multiplier;
    }
}
*/


/*
 * Как я понял Emit запускает выполнение IL кода
 */

using System.Reflection;
using System.Reflection.Emit;
using System.Threading.Channels;

Console.WriteLine("Начинаем создавать свой класс");
//Задаем имя сборки
AssemblyName aName = new AssemblyName("DynamicAssemblyExample");
AssemblyBuilder ab = AssemblyBuilder.DefineDynamicAssembly(aName, AssemblyBuilderAccess.Run);
//Создаем модуль (его имя должно быть такимже как имя сборки)
ModuleBuilder mb = ab.DefineDynamicModule(aName.Name!);
//Создаем наш класс
TypeBuilder tb = mb.DefineType("MyDynamicType", TypeAttributes.Public);
//Начинаем заполнять класс сущностями
//Определяем поле для класса с именем "m_number"
FieldBuilder fbNumber = tb.DefineField("m_number", typeof(int), FieldAttributes.Private);

//Создаем конструктор для класса
//Определение типов, которые будет принимать конструктор
Type[] parameterTypes = { typeof(int) };
ConstructorBuilder ctor1 = tb.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, parameterTypes);

ILGenerator ctor1IL = ctor1.GetILGenerator();
//Для конструктора нулевой элемент это ссылка на новый объект, 
//необходимо вызвать пустой конструктор перед вызовом основного конструктора класса
ctor1IL.Emit(OpCodes.Ldarg_0);
ctor1IL.Emit(OpCodes.Call, typeof(object).GetConstructor(Type.EmptyTypes)!);
//Кладем экземпляр в стэк перед отправкой аргумента, который должен быть присвоен полю "m_number"
ctor1IL.Emit(OpCodes.Ldarg_0);
//Кладем в стек второй объект
ctor1IL.Emit(OpCodes.Ldarg_1);
//Заменяем значение в поле fbNumber значением из второго объекта помещенного в стэк (строка выше)
ctor1IL.Emit(OpCodes.Stfld, fbNumber);
//Возврат из текущего метода, помещая возвращаемое значение из стека вычислений вызываемого метода в стек вычислений вызывающего метода
ctor1IL.Emit(OpCodes.Ret);

ConstructorBuilder ctor0 = tb.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, Type.EmptyTypes);

ILGenerator ctor0IL = ctor0.GetILGenerator();
ctor0IL.Emit(OpCodes.Ldarg_0);
ctor0IL.Emit(OpCodes.Ldc_I4_S, 42);
ctor0IL.Emit(OpCodes.Call, ctor1);
ctor0IL.Emit(OpCodes.Ret);


//Определим проперти для назначения значения приватному полю
//Последний аргумент равен null потому что проперти не имеет параметров. 
PropertyBuilder pbNumber = tb.DefineProperty("Number", PropertyAttributes.HasDefault, typeof(int), null);
//Сет и Гет методы требуют установки специаольных аттрибутов
MethodAttributes getSetAttr = MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig;
//Определим метод Гет для Number.
//Метод возвращает int и не имеет параметров (помним, что Types.EmptyTypes можно заменить на null)
MethodBuilder mbNumberGetAccessor = tb.DefineMethod("get_Number", getSetAttr, typeof(int), null);
ILGenerator numberGetIL = mbNumberGetAccessor.GetILGenerator();

/*
 * Нулевым аргументом для свойства экземпляра является сам экземпляр
 * Загружаем экземпляр, затем загружаем приватное поле и выходим из метода оставив значение приватного поля в стеке
 */
numberGetIL.Emit(OpCodes.Ldarg_0);
numberGetIL.Emit(OpCodes.Ldfld, fbNumber);
numberGetIL.Emit(OpCodes.Ret);

/*
 * Установка Сеттера
 * Метод ничего не возвращает, но принимает в себя значение int
 */
MethodBuilder mbNumberSetAccessor = tb.DefineMethod("set_Number", getSetAttr, null,  new Type[]{typeof(int)});
ILGenerator numberSetIL = mbNumberSetAccessor.GetILGenerator();
/*
 * Передаем в стэк сам метод
 * Передаем в стэк аргумент
 * Сохраняем значение в стэке по адресу переменной fbNumber
 * Выходим из метода (сверху стэка остается метод)
 */
numberSetIL.Emit(OpCodes.Ldarg_0);
numberSetIL.Emit(OpCodes.Ldarg_1);
numberSetIL.Emit(OpCodes.Stfld, fbNumber);
numberSetIL.Emit(OpCodes.Ret);

// Маппим методы доступа Гет и Сет на пропертиБилдер. Проперти готово
pbNumber.SetGetMethod(mbNumberGetAccessor);
pbNumber.SetSetMethod(mbNumberSetAccessor);

MethodBuilder myMethod = tb.DefineMethod("MyMethod", MethodAttributes.Public, typeof(int), new Type[] { typeof(int) });
ILGenerator myMethIL = myMethod.GetILGenerator();
/*
 * Кладем в стек метод
 * Кладем в стек значение из приватной переменной fbNumber
 * Кладем в стек аргумент метода
 * Перемножаем 2 значения в стеке и кладем результат в стек
 * Выходим из метода, в стеке лежит результат вычислений, который потом будет использован вызывающим методом
 */
myMethIL.Emit(OpCodes.Ldarg_0);
myMethIL.Emit(OpCodes.Ldfld, fbNumber);
myMethIL.Emit(OpCodes.Ldarg_1);
myMethIL.Emit(OpCodes.Mul);
myMethIL.Emit(OpCodes.Ret);

// Завершаем создание типа
Type t = tb.CreateType();
/*
 * Т.к. AssemblyBuilderAccess имеет метод run, то код может быть выполнен немедленно.
 * Запускаем рефлексию для получения объектов
 */
MethodInfo mi = t.GetMethod("MyMethod");
PropertyInfo pi = t.GetProperty("Number");

//Получаем экземпляр своего типа испоьзуя конструктор по умолчанию
object o1 = Activator.CreateInstance(t);
//Смотрим что лежит в поле Number
Console.WriteLine($"o1.Number:{pi.GetValue(o1, null)}");
//Устанавливаем значение в поле Number
pi.SetValue(o1, 127, null);
Console.WriteLine($"o1.Number:{pi.GetValue(o1, null)}");

object[] arguments = { 22 };
//Выполняем метод и выводим результат
Console.WriteLine($"o1.MyMethod(22): {mi.Invoke(o1,arguments)}");

object o2 = Activator.CreateInstance(t, new object?[] { 5280 });
Console.WriteLine($"o2.Number: {pi.GetValue(o2, null)}");

