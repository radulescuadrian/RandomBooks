﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RandomBooks.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BookController : ControllerBase
{
    private readonly IBookService _bookService;

    public BookController(IBookService bookService)
    {
        _bookService = bookService;
    }

    #region Dropdowns
    [HttpGet("types")]
    public async Task<ActionResult<ServiceResponse<List<BookType>>>> GetBookTypes()
    {
        var result = await _bookService.GetBookTypes();
        return Ok(result);
    }

    [HttpGet("authors")]
    public async Task<ActionResult<ServiceResponse<List<BookType>>>> GetBookAuthors()
    {
        var result = await _bookService.GetAuthors();
        return Ok(result);
    }

    [HttpGet("languages")]
    public async Task<ActionResult<ServiceResponse<List<BookType>>>> GetBookLanguages()
    {
        var result = await _bookService.GetLanguages();
        return Ok(result);
    }
    #endregion

    [HttpGet("{bookId}")]
    public async Task<ActionResult<ServiceResponse<Book>>> GetBook(int bookId)
    {
        var result = await _bookService.GetBook(bookId);
        return Ok(result);
    }

    [HttpGet("~/api/Books/{page}")]
    public async Task<ActionResult<ServiceResponse<BookListResult>>> GetBooks(int page)
    {
        var result = await _bookService.GetBooks(page);
        return Ok(result);
    }

    [HttpGet("~/api/Books/{category}/{page}")]
    public async Task<ActionResult<ServiceResponse<BookListResult>>> GetBooksByCategory(string category, int page)
    {
        var result = await _bookService.GetBooksByCategory(category, page);
        return Ok(result);
    }

    [HttpGet("featured/{page}")]
    public async Task<ActionResult<ServiceResponse<BookListResult>>> GetFeaturedBooks(int page = 1)
    {
        var result = await _bookService.GetFeaturedBooks(page);
        return Ok(result);
    }

    [HttpGet("admin/{page}"), Authorize(Roles = "Admin")]
    public async Task<ActionResult<ServiceResponse<BookListResult>>> GetAdminBooks(int page = 1)
    {
        var result = await _bookService.GetAdminBooks(page);
        return Ok(result);
    }

    [HttpPost, Authorize(Roles = "Admin")]
    public async Task<ActionResult<ServiceResponse<Book>>> AddBook(BookEdit edit)
    {
        var result = await _bookService.AddBook(edit);
        return Ok(result);
    }

    [HttpPut, Authorize(Roles = "Admin")]
    public async Task<ActionResult<ServiceResponse<Book>>> UpdateBook(BookEdit edit)
    {
        var result = await _bookService.UpdateBook(edit);
        return Ok(result);
    }

    [HttpGet("search/{searchText}/{page}")]
    public async Task<ActionResult<ServiceResponse<BookListResult>>> SearchBooks(string searchText, int page = 1)
    {
        var result = await _bookService.SearchBooks(searchText, page);
        return Ok(result);
    }

    [HttpGet("searchsuggestions/{searchText}")]
    public async Task<ActionResult<ServiceResponse<List<string>>>> GetBookSearchSuggestions(string searchText)
    {
        var result = await _bookService.GetBookSearchSuggestions(searchText);
        return Ok(result);
    }
}
