/// <reference path="~/scripts/jquery.js" />
/// <reference path="~/scripts/ww.jquery.js" />

$(document).ready(function() {
    // Initialize statusbar
    showStatus({ afterTimeoutText: "Ready" });
    showStatus("Ready");

    // Load the item template into a cached global variable
    template = $("#item_template").html();

    loadBooks();
});

var bookPk = -1;
var activeBook = null;
var template = null;

function removeBook(ctl,ev)
{        
    var jBookItem = $(ctl).parents("#divBookItem");
    var pk = jBookItem.attr("tag");

    var e = $.event.fix(ev);    // turn into jQuery event
    e.stopPropagation();    
    
    showProgress();
    Proxy.DeleteBook(parseInt(pk), 
                     function(result) 
                    {
                        showProgress(true);
                        showStatus("Book deleted.",5000);
                        
                        jBookItem.fadeOut(1000, 
                                          function() { 
                                            jBookItem.remove(); 
                                          });
                    },onPageError);
                                     
    return false;                                                
}
function editNewBook(ctl) {
        
    editBook(ctl,-1);
}
// Pass -1 new book to edit
// Pass ctl to allow us to retrieve the embedded PK value off the row
function editBook(ctl,pk) {
    
    var jItem = $(ctl);
    
    // find ID through ScriptVariables component that creates Id properties (Id postfix)
    var jEditBook = $("#" + scriptVars.panEditBookId);
    
    if (pk)
      bookPk = pk;
    else
    {        
        bookPk = parseInt( jItem.attr("tag") );
    }
    if (bookPk < 1)
    {
        activeBook = scriptVars.emptyBook;
        updateBookForm(activeBook);
        $("#btnShowAmazonPicklist").attr("disabled", "");
        jEditBook.show();

        showAmazonList();
    }
    else
        $("#btnShowAmazonPicklist").attr("disabled","disabled");

    showProgress();
    Proxy.GetBook(bookPk,
                  function(book) {
                        showProgress(true);
                      jEditBook.show(); 
                      updateBookForm(book);
                  },
                  onPageError);
}
// Databind the book
function updateBookForm(book)
{
    activeBook = book;
    
    $("#" + scriptVars.txtTitleId).val( book.Title || "" );
    $("#" + scriptVars.txtAuthorId).val( book.Author || "" ); 
    $("#" + scriptVars.txtAmazonUrlId).val( book.AmazonUrl || "" );
    $("#" + scriptVars.txtAmazonImageId).val( book.AmazonImage || "" );
    $("#" + scriptVars.chkHighlightId).attr("checked",book.Highlight );
    $("#" + scriptVars.txtSortOrderId).val(book.SortOrder || "0");
    if (book.AmazonImage)
        $("#imgAmazonImage").show().attr("src", book.AmazonImage);
    else
        $("#imgAmazonImage").hide();
}
function saveBook(ctl)
{
    var jItem = $(ctl);
    
    var book = activeBook;

    // must assign the Pk reference
    book.Pk = bookPk;
    if (bookPk < 1)
       bookPk = -1;
    
    book.Title = $("#" + scriptVars.txtTitleId).val();
    book.Author = $("#" + scriptVars.txtAuthorId).val();
    book.AmazonUrl =  $("#" + scriptVars.txtAmazonUrlId).val();
    book.AmazonImage = $("#" + scriptVars.txtAmazonImageId).val();
    book.SortOrder = $("#" + scriptVars.txtSortOrderId).val();
    book.Highlight = $("#" + scriptVars.chkHighlightId).attr("checked");
    book.Category = $("#" + scriptVars.txtBookCategoryId).val();

    book.Description = "";
        
    if(book.SortOrder)
        book.SortOrder = parseInt( book.SortOrder);
    
    showProgress();
    Proxy.SaveBook(book,
                   function(savedPk) {
                       showProgress(true);
                       showStatus("Book has been saved.", 5000);
                       $("#" + scriptVars.panEditBookId).hide();
                       book.Pk = savedPk;
                       updateBook(book, true);

                       if (bookPk == -1)
                           $("#divBookListWrapper").scrollTop(9999);
                   },
                   onPageError);
}
function loadBooks() {

    showProgress();
    
    // Clear the content
    $("#divBookListWrapper").empty();
    
    
    var filter = $("#" + scriptVars.lstFiltersId).val();
    
    Proxy.GetBooks(filter, function(books) {
        //  creates one new item <div> and adds it to the container
        $(books).each(function(i) {
            updateBook(this); 
            showProgress(true); 
        });
    }, onPageError);    
}
function updateBook(book,highlight)
{    
    /// <summary>
    /// Updates an individual book entry in the list display
    /// </summary>    
    /// <param name="" type="var"></param>    
    /// <param name="" type="var">
    /// </param>
    /// <returns type="" />    

    // try to retrieve the single item in the list by tag attribute id
    var item = $(".bookitem[tag=" +book.Pk +"]");

    // grab and evaluate the template
    
    var html = parseTemplate(template, book);

    var newItem = $(html)
                    .attr("tag", book.Pk.toString())
                    .click(itemClickHandler)
                    .closable({ closeHandler: function(e) {
                        removeBook(this, e);
                    },
                        imageUrl: "../../css/images/remove.gif"
                    });
//                    
    if (item.length > 0) 
        item.after(newItem).remove();        
    else 
        newItem.appendTo($("#divBookListWrapper"));
    

    if (highlight) {
        newItem
            .addClass("pulse")
            .effect("bounce", { distance: 15, times: 3 }, 400);
        setTimeout(function() { newItem.removeClass("pulse"); }, 1200);            
    }

}
function itemClickHandler(e) {    
    var pk = $(this).attr("tag");
    editBook(this, parseInt(pk));    
}
var bookList = [];
function showAmazonList() {
    $("#" + scriptVars.panBookListId).show().maxZIndex();    
    var search = $("#" + scriptVars.txtSearchBooksId).val();
    if (!search)
       return;

    showProgressSearch();

    Proxy.GetAmazonItems(search,
                          $("#" + scriptVars.radSearchTypeId + " input:checked").val(),
                          $("#" + scriptVars.txtAmazonGroupId).val(),
                          function(matches) {

                              showProgressSearch(true);
                              bookList = matches;

                              var template = $("#amazon_item_template").html();
                              var html = parseTemplate(template, bookList);

                              $("#divBookList_Content").empty().append($(html));
                              //$(html).append

                          },
                          onPageError);
}

function selectBook(ctl)
{
    var item = $(ctl);
    var id = item.attr("tag");
    
    for(var i=0; i < bookList.length; i++) {
        
        item = bookList[i];
        if (item.Id != id)
            continue;
        
        book = scriptVars.emptyBook;

        // map fields from service to a book        
        book.Title = item.Title;
        book.Author = item.Author;
        book.AmazonUrl = item.ItemUrl;
        book.AmazonImage = item.ImageUrl;
        book.AmazonSmallImage = item.SmallImageUrl;
        book.Published = item.PublicationDate;
        book.Description = item.Abstract;
        
        updateBookForm(book);        
        
        $("#" + scriptVars.panBookListId).hide();                                                            
    }
}
function updateSortOrder(ctl,event)
{
    var ctl = $(ctl);
    
    if (!ctl.data("Sort"))
    {
        $("#divBookListWrapper").sortable(
        {
            opacity: 0.7,
            revert: true,
            scroll: true,
            containment: "parent",
            start: function(e) {
                // have to remvoe click handler off item so drop doesn't click
                //$(e.originalTarget).unbind("click");
                $(".bookitem").unbind("click");
        },
            stop: function(e) {
                $(ctl).data("Sort", "1");
                $(ctl).html($(ctl).html().replace("Sort List", "Update Sort"));

                // reattach the item click handler
                //$(e.originalTarget).click(itemClickHandler);
                $(".bookitem").click(itemClickHandler);
            }
        });    
            
        showStatus("List is sortable. Click on the Update Sort button to save.",3000);   
        return;                    
    }
        
    var items = [];
    $(".bookitem").each( function(i) {
        var itm = $(this);        
        items.push(parseInt( itm.attr("tag")) );                
    });

    showProgress();
    Proxy.UpdateSortOrder(items, function(result) {
        showProgress(true);
        showStatus("Sort order updated.", 3000);
        ctl.removeData("Sort");
        $(ctl).html($(ctl).html().replace("Update Sort", "Sort List"));
        loadBooks();  // refresh list so we get all the updated sort ids into list
    },
                                  onPageError);
}
function showProgress(hide)
{
    if (hide)
        $("#divProgress").hide();
    else
        $("#divProgress").show();
}
function showProgressSearch(hide)
{    
    if (hide)
        $("#divProgressSearch").hide();
    else
        $("#divProgressSearch").show();
}
function onPageError(error)
{
    // hide animations
    showProgress(true);
    showProgressSearch(true);
    
    showStatus( error.message, 5000, false, true );
}

$.fn.pulse = function(time) {
    if (!time)
        time = 1500;
    
    // this == jQuery object that contains selections
    $(this).addClass("pulse").fadeTo(time, 0.40,
                function() {                    
                    $(this).fadeTo(time / 2, 1, function() { $(this).removeClass("pulse") });
                });

    return this;
}
