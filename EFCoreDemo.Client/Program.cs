using EFCoreDemo.Client;
using Newtonsoft.Json.Linq;

var client = new swaggerClient("https://localhost:5001", new HttpClient());

// All

Console.WriteLine();
Console.WriteLine("列出所有資料：");
Console.WriteLine();

var data = await client.GetCourseAllAsync();

foreach (var item in data)
{
    Console.WriteLine(item.CourseId + "\t" + item.Title);
}

// By Id

Console.WriteLine();
Console.WriteLine("列出單筆資料：");
Console.WriteLine();

var course = await client.GetCourseByIdAsync(1);

Console.WriteLine(course.CourseId + "\t" + course.Title);

// Create

Console.WriteLine();
Console.WriteLine("建立資料：");
Console.WriteLine();

var newCourse = new CourseCreateDto
{
    Title = "New Course",
    Credits = 6,
    DepartmentId = 1
};

try
{
    var result = await client.CreateCourseAsync(newCourse);
    Console.WriteLine(result.CourseId + "\t" + result.Title);
}
catch (EFCoreDemo.Client.ApiException<EFCoreDemo.Client.ProblemDetails> ex)
{
    foreach (var (key, value) in ex.Result.AdditionalProperties.ToList())
    {
        //Console.WriteLine(key);
        //Console.WriteLine(value);
        if (key is "errors")
        {
            Console.WriteLine();
            Console.WriteLine("建立資料發生錯誤：");
            Console.WriteLine();

            JObject val = (JObject)value; // { "Credits": [ "課程評價必須介於 1 到 5 之間" ]}
            foreach (var (fieldName, errors) in val)
            {
                if (errors is JArray errMsgs)
                {
                    foreach (var errMsg in errMsgs)
                    {
                        Console.WriteLine($"- {fieldName}\t{errMsg}");
                    }
                }
            }
        }
    }
    //throw ex;
}

// All
Console.WriteLine();
Console.WriteLine("重新列出所有資料：");
Console.WriteLine();

var data2 = await client.GetCourseAllAsync();

foreach (var item in data2)
{
    Console.WriteLine(item.CourseId + "\t" + item.Title);
}
