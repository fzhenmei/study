$(document).ready(function() {
    var url = "http://twitter.com/status/user_timeline/RickStrahl.json?count=5&callback=jsonpCallback";
    jsonp(url);
});

this.jsonp = function(url) {
    $.getScript(url);
}
function jsonpCallback(result) {
    
    if (!result)
        return;  // odd bug - called twice once with a DOM object)
    var tweets = $("#tweets");    
    for (var i=0;i<result.length;i++) {
        var item = result[i];
               if (i == 0) {
                    
                   $("<div>")
                       .append($("<img />").attr("src", item.user.profile_image_url).attr("align", "left").css("margin-right",5) )
                       .append("<h3>" + item.user.name + "</h3>")
                       .append($("<img />").attr("src", scriptVars.webBasePath + "images/twitter-chicklet.jpg"))
                       .append("<br clear='all'/>")
                       .css( {margin: "3px 0px 5px",
                              background: "lightblue",
                              cursor: "pointer"})                            
                       .appendTo(tweets)
                       .click(function() { window.location='http://www.twitter.com/rickstrahl'; });
               }
        
        
        var txt = item.text;
        txt = txt.replace(/@([\w|\d]*)/, "<a href='http://www.twitter.com/$1'>@$1</a>");
        $("<div>")
                .addClass("twitteritem")
                .html(txt)
                .appendTo(tweets);
        $("<div>")
                .addClass("twitterbyline")
                .html("about " + relative_time(item.created_at) + " ago")
                .appendTo(tweets);

    }
}
function relative_time(time_value) {
    
    var values = time_value.split(" ");
    time_value = values[1] + " " + values[2] + ", " + values[5] + " " + values[3];
    var parsed_date = Date.parse(time_value);
    var relative_to = (arguments.length > 1) ? arguments[1] : new Date();
    var delta = parseInt((relative_to.getTime() - parsed_date) / 1000);
    delta = delta + (relative_to.getTimezoneOffset() * 60);    
    return (delta < 3600) ? Math.round(delta / 60).toString() + "m" :
	                            Math.round(delta / 3600).toString() + "h";
}

