1. 表达式树可以建立动态linq查询。
```cs
    class Program
    {
        static void Main(string[] args)
        {
            BindAndSort("Name");
            BindAndSort("Email");
            Console.Read();
        }

        private static void BindAndSort(string pro)
        {
            List<Person> people = new List<Person>
            {
                new Person(){ Name = "Pranay",Email="pranay@test.com",CityID=2 },
                new Person(){ Name = "Heamng",Email="Hemang@test.com",CityID=1 },
                new Person(){ Name = "Hiral" ,Email="Hiral@test.com",CityID=2},
                new Person(){ Name = "Maitri",Email="Maitri@test.com",CityID=1 }
            };

            ParameterExpression param = Expression.Parameter(typeof(Person), "People");

            Expression ex = Expression.Property(param, pro);

            var sort = Expression.Lambda<Func<Person, object>>(ex, param);

            var sortedData =
                            (from s in people
                             select s).OrderBy<Person, object>(sort.Compile()).ToList<Person>();

            foreach (var person in sortedData)
            {
                Console.WriteLine($"{person.Name} {person.Email} {person.CityID}");
            }
        }
    }
```
查询语法及方法链语法
[相关资源](https://www.simple-talk.com/dotnet/.net-framework/dynamic-linq-queries-with-expression-trees/)
获取特定类型特定属性的委托
```cs
        public static Func<TSource, object> GenericEvaluateOrderBy<TSource>
            (string propertyName)
        {
            var type = typeof(TSource);
            var parameter = Expression.Parameter(type, "p");
            var propertyReference = Expression.Property(parameter,
                    propertyName);
            return Expression.Lambda<Func<TSource, object>>
                    (propertyReference, new[] { parameter }).Compile();
        }
    ```
    
