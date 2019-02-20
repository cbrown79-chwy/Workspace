
# Leaky Dependencies

Your simplest .NET solution - 1 project.

This is the easiest solution to conceptualize.

We'll call it ProjectAlpha.  ProjectAlpha is a single project solution. It has an exe output. Maybe it's a long running Windows Service (daemon for you non-Windows folks), or a simple console app, and it is entirely self-contained.

ProjectAlpha should be a simple project, with maybe a few complex classes / functions that encapsulate the bulk of the functionality required.  Like most applications, it largely takes data in, validates and converts it into domain objects of some sort, does the "work", then returns some sort of reasonable response.

Generally, you don't need to do much for these sorts of projects in terms of dependency management. You just load whatever dependencies you need, and load accordingly.

Think of your application as an entry point (generally, the EXE, but in Web apps, the native dlls that are hosting the site itself), and that dependency lines should be largely like a simplified set of branches coming into that entry point.

What is the purpose a DLL? To share code between multiple user entry points.

Think to yourself when writing your DLL… what would I need to add to make an entirely new entry point that consumes this? Do I NEED another 3rd party library? Would I HAVE to install something else just to get to reuse this code?

Expect someone to make a simple binary reference to the DLL, and use it. Will it work without them also installing the rest of your application?

Why would this ever matter?

Re-use is an important element. Our job is to put pieces together, not reinvent everything.
Minimizing risk of changing things. I.E. Changing things is EASIER when you depend on less stuff.
Performance improvements (code not intended to be re-used should be put in the entry point assembly.)
Simplifying testing.
Eyeball tests get more obvious.
Principals: Write your code to be reused and demand very little from those that re-use it.

Dependency leakage.

Naturally, some dependencies are simply GOING to leak (System, or System.Data). Your job is basically to determine if they SHOULD. If we have a company policy, for example, to use log4net, feel free to make references to it all over, BUT consider installing it into the GAC and referencing the GAC'd version. You'll gain performance with JIT'ed logging code, and you minimize deployment package size.

If you bristle at that idea, ask yourself why. Is it because that takes away your ability to make changes in your app? That it seems risky to pin an app on a particular version / technology?  Well, that's exactly why you minimize dependencies entirely!

What about general dependencies though? Nodatime for Date/Time management, or JSON.net, or <_Insert Your Favorite DI Container_>? We all generally approve of those things, so why shouldn't they be allowed to leak?

The first question comes down to can you change it quickly, if you need to. What if Nodatime is actually a bitcoin miner under the hood, or a keystroke logger? What if that fancy new FLURL library doesn't migrate to .NET core, because the author migrated to writing Go, and doesn't give a crap about .NET anymore?