CsProjSorter
============

Simple utility for alpha-sorting CSPROJ files used by Visual Studio to help make resolving merge conflicts less... Eye-gougey.

When you have a team working on several features at a time on their own branches, it can often be a real pain to merge/rebase since handling XML is inherently a terrible experience since most merge algorithms just can't cope with it.

While we can take steps to reduce the pain (short-live branches, pull from `master` regular etc.) problems can (and will) still occur.

The idea behind this is to ensure that the project file(s) that are on `master` are always alpha-sorted, this way it can at least be _easier_ to see what the hell is going on, as well as reduce the merge conflicts just by virtue of the fact that same-line changes will be much rarer.

I'm thinking that those with commit access to `master` on the nominated central repository can also setup things like a pre-commit hook to ensure this "just happens" when they commit to `master`.

Anyway - let's see how this pans out :smile: 

### ROADMAP

- Collapse multiple `ItemGroup` nodes (with the same 'Child' types - `Compile`, `Reference` etc) into a single node.
- Ensure that duplicate items are removed.
- Get CLI created so we can just throw a CSPROJ file at it.
- Add validation of the document at the end to check for things like unclosed nodes etc.

### THOUGHTS

- Do we want to establish some kind of standard around the ordering of the top-level nodes? (e.g. `Import`, `PropertyGroup` etc.) Even if we just use the "Anatomy" image on the [ASP.NET site](http://www.asp.net/web-forms/tutorials/deployment/web-deployment-in-the-enterprise/understanding-the-project-file).
