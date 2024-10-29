using Microsoft.AspNetCore.Http;
using DiffCalculatorApi.Endpoints;
using System.Net.Http.Json;
using DiffCalculatorApi.Models;
using DiffCalculatorApi.ViewModels;
using System.Net;
using DiffCalculatorTests.Infrastructure;

namespace DiffCalculatorTests;

public class IntegrationTests(CustomTestFixture _fixture) : IClassFixture<CustomTestFixture>
{
    private const int id = 1;

    [Fact]
    public async Task AddRight_Null_Data_Bad_Request()
    {
        var response = await _fixture.Client.PutAsJsonAsync($"/v1/diff/{id}/right", new DiffRequestData
        {
            Data = null
        });

        Assert.False(response.IsSuccessStatusCode);
        Assert.IsType<HttpResponseMessage>(response);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("Input cannot be null or empty", await response.Content.ReadFromJsonAsync<string>());
    }

    [Fact]
    public async Task AddRight_Success()
    {
        DiffRequestData diffRequestData = new()
        {
            Data = "AAAAAA=="
        };

        var response = await _fixture.Client.PutAsJsonAsync($"/v1/diff/{id}/right", diffRequestData);

        Assert.True(response.IsSuccessStatusCode);
        Assert.IsType<HttpResponseMessage>(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Empty(await response.Content.ReadAsStringAsync());
    }

    [Fact]
    public async Task AddLeft_Success()
    {
        DiffRequestData diffRequestData = new()
        {
            Data = "AAAAAA=="
        };

        var response = await _fixture.Client.PutAsJsonAsync($"/v1/diff/{id}/left", diffRequestData);

        Assert.True(response.IsSuccessStatusCode);
        Assert.IsType<HttpResponseMessage>(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Empty(await response.Content.ReadAsStringAsync());
    }

    [Fact]
    public async Task AddLeft__NotValidBase64String()
    {
        DiffRequestData diffRequestData = new()
        {
            Data = "notbase64"
        };

        var response = await _fixture.Client.PutAsJsonAsync($"/v1/diff/{id}/left", diffRequestData);
        Assert.False(response.IsSuccessStatusCode);
        Assert.IsType<HttpResponseMessage>(response);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("The input is not a valid Base - 64 string", await response.Content.ReadFromJsonAsync<string>());
    }

    [Fact]
    public async Task GetDiff_NotFound()
    {
        _fixture.UseRealRepository();

        var response = await _fixture.Client.GetAsync($"/v1/diff/{id}");
        Assert.False(response.IsSuccessStatusCode);
        Assert.IsType<HttpResponseMessage>(response);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Empty(await response.Content.ReadAsStringAsync());
    }

    [Fact]
    public async Task GetDiff_Equals()
    {
        _fixture.MockRepository.Setup(x => x.GetRight(id)).Returns(new Right(1, Convert.FromBase64String("AAAAAA==")));
        _fixture.MockRepository.Setup(x => x.GetLeft(id)).Returns(new Left(1, Convert.FromBase64String("AAAAAA==")));

        var response = await _fixture.Client.GetAsync($"/v1/diff/{id}");
        Assert.True(response.IsSuccessStatusCode);
        Assert.IsType<HttpResponseMessage>(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<DiffResult>();
        Assert.NotNull(result);
        Assert.Equal("Equals", result.DiffResultType);
        Assert.Empty(result.Diffs);
    }

    [Fact]
    public async Task GetDiff_ContentDoNotMatch()
    {
        _fixture.CreateNewMock();
        _fixture.MockRepository.Setup(x => x.GetRight(id)).Returns(new Right(id, Convert.FromBase64String("bGlaaHBgd29yAy5=")));
        _fixture.MockRepository.Setup(x => x.GetLeft(id)).Returns(new Left(id, Convert.FromBase64String("bGlnaHQgd29yay4=")));

        var response = await _fixture.Client.GetAsync($"/v1/diff/{id}");
        Assert.True(response.IsSuccessStatusCode);
        Assert.IsType<HttpResponseMessage>(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<DiffResult>();
        Assert.NotNull(result);
        Assert.Equal("ContentDoNotMatch", result.DiffResultType);
        Assert.Equal(3, result.Diffs.Count);
        Assert.Equal(2, result.Diffs[0].Offset);
        Assert.Equal(1, result.Diffs[0].Length);
        Assert.Equal(4, result.Diffs[1].Offset);
        Assert.Equal(2, result.Diffs[1].Length);
        Assert.Equal(9, result.Diffs[2].Offset);
        Assert.Equal(1, result.Diffs[2].Length);
    }
}
