---
name: bump-version
description: Bumps the version number for the .NET Starter Project Template NuGet package. Use this skill whenever the user wants to bump, increment, update, or change the version of the template package. This includes patch releases, minor releases, major releases, or setting an explicit version. The skill updates both the PackageVersion in DotNetStarterProjectTemplate.Package.csproj and the version reference in the README.md installation command.
---

# Bump Version Skill

This skill bumps the template package version in two places:
1. `PackageVersion` property in `DotNetStarterProjectTemplate.Package.csproj`
2. The install command version in `README.md` (e.g. `dotnet new install MarcelMichau.Templates.DotNetStarterProject@X.Y.Z`)

## Process

### Step 1: Read current version

Read `DotNetStarterProjectTemplate.Package.csproj` and extract the current `PackageVersion` value.

### Step 2: Determine target version

If the user specified an exact version (e.g. "2.1.0"), use that directly.

If the user specified a bump type (patch/minor/major), calculate the new version from the current one:
- **patch**: increment the third number (2.0.19 → 2.0.20)
- **minor**: increment the second number, reset patch to 0 (2.0.19 → 2.1.0)
- **major**: increment the first number, reset minor and patch to 0 (2.0.19 → 3.0.0)

If the user hasn't specified what to bump, ask them: "Which part should I bump — patch, minor, or major? Or would you like to set an exact version?"

### Step 3: Update both files

**In `DotNetStarterProjectTemplate.Package.csproj`**, replace:
```xml
<PackageVersion>OLD_VERSION</PackageVersion>
```
with:
```xml
<PackageVersion>NEW_VERSION</PackageVersion>
```

**In `README.md`**, replace the version in the install command:
```
dotnet new install MarcelMichau.Templates.DotNetStarterProject@OLD_VERSION
```
with:
```
dotnet new install MarcelMichau.Templates.DotNetStarterProject@NEW_VERSION
```

### Step 4: Report changes

Summarise what was updated, e.g.:
```
Version bumped: 2.0.19 → 2.0.20

Updated files:
- DotNetStarterProjectTemplate.Package.csproj (PackageVersion)
- README.md (install command)
```
