namespace HabitTacker.Exceptions;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
     
        catch (ArgumentNullException ex)
        {
            var errors = new Dictionary<string, string[]>
            {
                { "argumentNull", new[] { ex.Message } }
            };
            await HandleExceptionAsync(context, StatusCodes.Status400BadRequest, "Invalid argument", errors);
        }
        
        catch (NotFoundException ex)
        {
            var errors = new Dictionary<string, string[]>
            {
                { "notFound", new[] { ex.Message } }
            };
            await HandleExceptionAsync(context, StatusCodes.Status404NotFound, "Resource not found", errors);
        }
       
        catch (KeyNotFoundException ex)
        {
            await HandleExceptionAsync(context, StatusCodes.Status500InternalServerError, ex.Message);
        }
        catch (NullValueException ex)
        {
            await HandleExceptionAsync(context, StatusCodes.Status400BadRequest, ex.Message);
        }
        catch(InvalidOperationException ex)
        {
            await HandleExceptionAsync(context, StatusCodes.Status400BadRequest, ex.Message);
        }
        catch (Exception)
        {
            await HandleExceptionAsync(context, StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, int statusCode, string msg, object? error = null)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";
        var response = new
    {
        success = false,
        message = msg,
        data = (object?)null,
        errors = error 
    };
        await context.Response.WriteAsJsonAsync(response);
    }
}
