using EFCoreDemo.Client;

var client = new swaggerClient("https://localhost:5001", new HttpClient());

// All

var data = await client.GetCourseAllAsync();

foreach (var item in data)
{
    Console.WriteLine(item.CourseId + "\t" + item.Title);
}

// By Id

var course = await client.GetCourseByIdAsync(1);

Console.WriteLine(course.CourseId + "\t" + course.Title);

// Create

var newCourse = new CourseCreateDto
{
    Title = "New Course",
    Credits = 4,
    DepartmentId = 1
};

var result = await client.CreateCourseAsync(newCourse);

Console.WriteLine(result.CourseId + "\t" + result.Title);

// All

var data2 = await client.GetCourseAllAsync();

foreach (var item in data2)
{
    Console.WriteLine(item.CourseId + "\t" + item.Title);
}
