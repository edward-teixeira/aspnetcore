// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading.Tasks;
using Microsoft.AspNetCore.Testing;
using Templates.Test.Helpers;
using Xunit;
using Xunit.Abstractions;

namespace Templates.Items.Test;

public class BlazorServerTest
{
    public BlazorServerTest(ProjectFactoryFixture projectFactory, ITestOutputHelper output)
    {
        ProjectFactory = projectFactory;
        Output = output;
    }

    public Project Project { get; set; }

    public ProjectFactoryFixture ProjectFactory { get; }
    public ITestOutputHelper Output { get; }

    [Fact]
    public async Task BlazorServerItemTemplate()
    {
        Project = await ProjectFactory.GetOrCreateProject("razorcomponentitem", Output);

        var createResult = await Project.RunDotNetNewAsync("razorcomponent --name Different");
        Assert.True(0 == createResult.ExitCode, ErrorMessages.GetFailedProcessMessage("create", Project, createResult));

        Project.AssertFileExists("Different.razor", shouldExist: true);
        Assert.Contains("<h3>Different</h3>", Project.ReadFile("Different.razor"));
    }
}
