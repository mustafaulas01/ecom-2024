using System;

namespace API;

public class ApiErrorResponse(int statusCode,string message,string details)
{

    public int StatusCode { get; set; }=statusCode;

    public string Message { get; set; }=message;

    public string? Details { get; set; }=details;

}
