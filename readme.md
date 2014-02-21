WTester - a scripting language for Selenium
===========================================

Firstly let me say that Selenium is amazing. If you don't know of this project then [check out browser automation here](http://docs.seleniumhq.org/). WTester builds on top of Selenium and gives you a simple scripting language to automate your browser instead of the slightly more complex Java or C# that you normally use. Sound good?

First, an apology
-----------------

WTester is currently Windows-only. It's written in .Net, and I haven't had chance to test in in Mono. If anyone wants to port it be my guest!

Some basic examples
-------------------

OK, so how simple is this script? Here's how to open a link to Google in Firefox:

`$.browser("firefox")`<br>
`$.load("www.google.com")`

You'll see that every line starts with a `$` then a dot. After that there's a whole bunch of commands you can use, but the first one **must** be `$.browser`. Depending on the browsers you have installed you can choose from:

- Firefox
- IE
- Chrome

These commands are called 'actions' and can automate loads of things in your browser. Here are just a few examples:

Get the document title:

`$.title()`

Click the button with ID 'submit':

`$.click("#submit")`

Find the input with the class 'firstname' and type 'Chris' into it:

`$.find(".firstname").typetext("Chris")`

Run some arbitrary JavaScript:

`$.eval("alert('Test');")`

For more details on all the actions available there's a help document called help.rtf - this file is also built in the GUI.

Once you've written your test you can save it as a .wtest file, which is just a text document that handles commenting in a JavaScript style, like this:

`// this is a comment`

Dead easy.

So how do you run it?
---------------------

WTester is available in two flavours: a GUI and a command line app. In the GUI you can write WTests to try them out, open .wtest files and run them and see the output. The command line app can run .wtest files and prints out the results - what has passed, what failed.

There's no integration with testing frameworks built in, but if you write an app to use WTester (of which the GUI and console apps are examples) you can use the built-in delegate methods for whatever you want. Heck, you can even write your own actions if you want.