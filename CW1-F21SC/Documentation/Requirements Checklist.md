Checklist for system requirements.

1.  Sending HTTP request messages for URLs typed by the user.

2.  Receiving HTTP response messages and display the contents of the messages on the interface.
    Note that you are only required to display the HTML code returned to the web browser from the
    web server (HTML parsing and graphical display are not required). In addition to the 200 OK
    Deadline: 30th October 2023 Page 1 of 4
    F20SC: Industrial Programming CW1: Web Browser (50%) 2023/2024
    HTTP response status code, the following HTTP response status error codes and the display of their
    corresponding error messages should be supported:
    – 400 Bad Request
    – 403 Forbidden
    – 404 Not Found
    To test these error codes, look up web pages like https://savanttools.com/test-http-status-codes.
3.  Display the HTTP response status code and (if applicable) the title of the web page at the top
    of the browser’s main window. Reload the current page by sending another HTTP request for
    the current web page, and display the contents of the page together with the title and the HTTP
    response status code as specified above.

4.  The user should be able to create and edit a home page URL. The Home page URL should be
    loaded on the browser’s start up, and it should be initialised with your university home page.
    
5.  The user should be able to add a URL for a web page requested to a list of favourite web pages.
    The user should also be able to associate a name with each favourite URL. Support for favourite
    items modification and deletion is required. The user should be able to request a favourite web page
    by clicking its name on the Favourites list. On the browser’s start up, the favourites list should be
    loaded to the browser.
    
6. The browser should maintain history, i.e., a list of URLs, corresponding to the web pages requested
    by the user. The user should be able to navigate to previous and next pages, and jump to a page by
    clicking on the links in the History list. On the browser’s start up, the history list should be loaded
    to the browser.
    
7. The application should provide a bulk download facility as follows. The user should be able specify
    a file name (default setting bulk.txt) containing URLs (exactly one per line). When initiating bulk
    download, the application should retrieve each of the pages listed in the file, and display the results
    as a list of lines of the form
    \<code\> \<bytes\> \<URL\>
    where \<code> should be the response status code from the page retrieval (as in 11Receiving HTTP
    response messages” above), \<bytes> should be the number of bytes retrieved from this URL, and
    \<URL> should be the URL as specified in the bulk download file. Only these results should be shown
    in the text area normally used for displaying the HTML code. One line should be shown for every
    entry in the bulk download file. No HTML contents should be displayed for this functionality.
    
8.  A simple GUI should be provided to perform the operations discussed above. The GUI should be
    implemented using either the Windows Forms (Windows) or the Gtk# (Linux) libraries. The GUI
    for the web browser should support the following:
    – Using the GUI, the user should be able to perform the operations discussed above.
    – Make use of menus (with appropriate shortcut keys) as well as buttons to increase accessibility.
    
Requirements Met:

[x]     1 - Send HTTP Request

[x]     2 - Receive HTTP Response

[x]     3 - Display HTTP Response Status Code

[x]     4 - Create and Edit Home Page URL

[x]     5 - Add URL to Favourites

[ ]     6 - Maintain History

[ ]     7 - Bulk Download Facility

[x]     8 - Simple GUI