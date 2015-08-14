# html-mvc-aspnet5
ASP.NET 5 demo of HTML MVC

This repository is supposed to demonstrate the principles of creating a server-rendered MVC application that utilizes content negotation and some peppered markup to create a rich, single-page application experience. It is being worked on in conjuction with [HTML MVC](https://github.com/tuespetre/html-mvc) and takes full advantage of new ASP.NET 5 features to ease the burden of attaining to the desired end.

## Concepts

**Relationships between models, views, controllers at the server**
- Views are a representation of a model or **resource** rather than the model itself, and thus
- Controllers speak purely in models, leaving content negotation to other mechanisms
- Every controller action (or URL if you prefer) has a 'primary model' or **resource** it wishes to expose; however,
- A given controller action may wish to provide 'auxiliary models' as well (think layout models)

**Structuring of views at the server, and data binding**
- Views at the server naturally form a structured tree where each node has a name (the name of the view)
- Model data will not cascade from a layout view to its layout dependent; [this can be done as an awful, putrefascent hack](https://gist.github.com/tuespetre/9f05b0bac11f95485a70) but probably *shouldn't* be
- ASP.NET 5 offers Tag Helpers, a truly clever hybrid of everything WebForms and Angular were going for, to allow the client-side binding expressions to be shared with the server, preventing duplication

**Particular notes about HTML MVC**
- Every binding expression is strictly a value; so you see the use of `NoItems` and `AnyItems` properties on the view model rather than `Items.length > 0`
- This allows for a very simple and functional, logic-free binding experience I have coined **decoration over mutation**.
- Everything binds at the server; the client does not do any binding or fetching at all until a link is clicked or a form is submitted
- Note that the client may do *some* initial fetching for views with a planned `<link rel="import view">` construct

**Possibilities**
- With the view structuring and model binding mechanisms offered by HTML MVC, it would be easy to conceive of something like a declarative API for model which is periodically refreshed (and the view re-bound), whether by interval-based fetching, websockets, or server-sent events
