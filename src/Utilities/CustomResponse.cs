namespace WebApi.Utilities;

/// <summary>
/// Represents a generic response from a service or an API.
/// </summary>
/// <typeparam name="T">The type of the data included in the response.</typeparam>
public class CustomResponse<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CustomResponse{T}"/> class.
    /// </summary>
    public CustomResponse() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomResponse{T}"/> class with the specified data.
    /// </summary>
    /// <param name="data">The data to be included in the response.</param>
    /// <exception cref="ArgumentNullException">Thrown when data is null.</exception>
    public CustomResponse(T data)
    {
        Data = data ?? throw new ArgumentNullException(nameof(data));
        Succeeded = true;
        Message = string.Empty;
        Errors = null;
    }

    /// <summary>
    /// Gets or sets the data included in the response.
    /// This is the main content of the response.
    /// </summary>
    public T? Data { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the operation that generated the response succeeded.
    /// True if the operation was successful; otherwise, false.
    /// </summary>
    public bool Succeeded { get; set; }

    /// <summary>
    /// Gets or sets an array of error messages, if any, associated with the response.
    /// This is typically used when the operation that generated the response failed or encountered issues.
    /// </summary>
    public string[]? Errors { get; set; }

    /// <summary>
    /// Gets or sets a message providing additional information about the response.
    /// This can be a success message, a description of an error, or any other relevant information.
    /// </summary>
    public string Message { get; set; } = string.Empty;
}