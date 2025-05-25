var builder = WebApplication.CreateBuilder(args);

// �[�J Session + TempData �䴩
builder.Services.AddSession();
builder.Services.AddRazorPages()
    .AddSessionStateTempDataProvider();

builder.Services.AddHttpClient<OpenAIService>();
builder.Services.AddSingleton<OpenAIService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseRouting();

app.UseSession(); // �}�� Session �䴩

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
