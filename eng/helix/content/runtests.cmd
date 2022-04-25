@ECHO OFF
SETLOCAL

REM Use '$' as a variable name prefix to avoid MSBuild variable collisions with these variables
set $target=%1
set $aspRuntimeVersion=%2
set $queue=%3
set $arch=%4
set $quarantined=%5
set $ef=%6
set $helixTimeout=%7
set $installPlaywright=%8
REM Batch only supports up to 9 arguments using the %# syntax, need to shift to get more

set DOTNET_SKIP_FIRST_TIME_EXPERIENCE=1
set DOTNET_MULTILEVEL_LOOKUP=0
set InstallPlaywright=%$installPlaywright%
set PLAYWRIGHT_BROWSERS_PATH=%CD%\ms-playwright

set "PATH=%HELIX_WORKITEM_ROOT%;%PATH%;%HELIX_WORKITEM_ROOT%\node\bin"
echo Set path to: "%PATH%"
echo.

set exit_code=0

move NuGet.config NuGet.config.save
echo "Trace: dotnet tool install --tool-path %CD% --add-source %CD%\\ dotnet-trace"
dotnet tool install --tool-path %CD% --add-source %CD%\\ dotnet-trace
set exit_code=%errorlevel%
move NuGet.config.save NuGet.config

if %exit_code% NEQ 0 (
    echo "dotnet tool install failed: exit_code=$exit_code"
    EXIT /b %exit_code%
)

echo "Restore: dotnet restore RunTests\RunTests.csproj"
.\dotnet-trace.exe collect --providers "System.Net.Http,System.Net.Sockets,System.Net.Security,System.Net.NameResolution" -- dotnet restore RunTests\RunTests.csproj --verbosity detailed

if not errorlevel 0 (
    set exit_code=%errorlevel%
    echo "Restore runtests failed: exit_code=%exit_code%"
    COPY *.nettrace %HELIX_WORKITEM_UPLOAD_ROOT%\\
    EXIT /b %exit_code%
)

echo "Running tests: dotnet run --no-restore --project RunTests\RunTests.csproj -- --target %$target% --runtime %$aspRuntimeVersion% --queue %$queue% --arch %$arch% --quarantined %$quarantined% --ef %$ef% --helixTimeout %$helixTimeout%"
dotnet run --no-restore --project RunTests\RunTests.csproj -- --target %$target% --runtime %$aspRuntimeVersion% --queue %$queue% --arch %$arch% --quarantined %$quarantined% --ef %$ef% --helixTimeout %$helixTimeout%
if not errorlevel 0 (
    set exit_code=%errorlevel%
)
echo "Finished running tests: exit_code=%exit_code%"
EXIT /b %exit_code%
