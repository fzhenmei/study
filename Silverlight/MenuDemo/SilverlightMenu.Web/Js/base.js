/*
*	函数说明：创建一个类，等于C#的类
*/
var Class = {
  create: function() {
    return function() {
      this.initialize.apply(this, arguments);
    }
  }
}
//--------------------------------开始------------------------------
/*
 * 函数说明：取出对象，等于document.getElementById()
 * 参数：	 对象名，参数可以多个，用逗号隔开
 * 返回值：	对象
 */
function $() {
  var elements = new Array();
  
  for (var i = 0; i < arguments.length; i++) {
    var element = arguments[i];
    if (typeof element == 'string')
      element = document.getElementById(element);

    if (arguments.length == 1) 
      return element;
      
    elements.push(element);
  }
  
  return elements;
}
//--------------------------------结束-----------------------------


//--------------------------------开始------------------------------
/*
*  函数说明:取出对象的值，等于document.getElementById('').value
*/
var Form = {
  serialize: function(form) {
    var elements = Form.getElements($(form));
    var queryComponents = new Array();
    
    for (var i = 0; i < elements.length; i++) {
      var queryComponent = Form.Element.serialize(elements[i]);
      if (queryComponent)
        queryComponents.push(queryComponent);
    }
    
    return queryComponents.join('&');
  },
  
  getElements: function(form) {
	form = $(form);
    var elements = new Array();

    for (tagName in Form.Element.Serializers) {
      var tagElements = form.getElementsByTagName(tagName);
      for (var j = 0; j < tagElements.length; j++)
        elements.push(tagElements[j]);
    }
    return elements;
  },
  
  disable: function(form) {
    var elements = Form.getElements(form);
    for (var i = 0; i < elements.length; i++) {
      var element = elements[i];
      element.blur();
      element.disable = 'true';
    }
  },

  focusFirstElement: function(form) {
    form = $(form);
    var elements = Form.getElements(form);
    for (var i = 0; i < elements.length; i++) {
      var element = elements[i];
      if (element.type != 'hidden' && !element.disabled) {
        Field.activate(element);
        break;
      }
    }
  },

  reset: function(form) {
    $(form).reset();
  }
}
Form.Element = {
  serialize: function(element) {
    element = $(element);
    var method = element.tagName.toLowerCase();
    var parameter = Form.Element.Serializers[method](element);
    
    if (parameter)
      return encodeURIComponent(parameter[0]) + '=' + 
        encodeURIComponent(parameter[1]);                   
  },
  
  getValue: function(element) {
    element = $(element);
    var method = element.tagName.toLowerCase();
    var parameter = Form.Element.Serializers[method](element);
    
    if (parameter) 
      return parameter[1];
  }
}

Form.Element.Serializers = {
  input: function(element) {
    switch (element.type.toLowerCase()) {
      case 'hidden':
      case 'password':
      case 'text':
        return Form.Element.Serializers.textarea(element);
      case 'checkbox':  
      case 'radio':
        return Form.Element.Serializers.inputSelector(element);
    }
    return false;
  },

  inputSelector: function(element) {
    if (element.checked)
      return [element.name, element.value];
  },

  textarea: function(element) {
    return [element.name, element.value];
  },

  select: function(element) {
    var index = element.selectedIndex;
    var value = element.options[index].value || element.options[index].text;
    return [element.name, (index >= 0) ? value : ''];
  }
}
var $F = Form.Element.getValue;
//--------------------------------------结束--------------------------------------------


//--------------------------------------开始--------------------------------------------
/*
* 函数说明：文本框首字符不允许包含数字
*/
function isContainNumber(elementID)
{
	var number_chars = "1234567890";
	var str = $(elementID).value;
	if(str.length !=0)
	{								
		if (number_chars.indexOf(str.charAt(0))!=-1) 
		{						
			$(elementID).focus();							
			return true;
		}		
	}
	return false;
}
//--------------------------------------结束--------------------------------------------

function XCookie()
{	
	XCookie.SetCookie = function(sName, sValue, nExpireSec, sDomain, sPath)
	{
		var sCookie = sName+"="+sValue+";";	
		if (nExpireSec)
		{
			var oDate = new Date();
			oDate.setTime(oDate.getTime()+parseInt(nExpireSec)*1000);
			sCookie += "expires="+oDate.toUTCString()+";";
		}
		if (sDomain)
		{
			sCookie += "domain="+sDomain+";";
		}
		if (sPath)
		{
			sCookie += "path="+sPath+";"
		}
		
		document.cookie = sCookie;
	}
	
	XCookie.GetCookie = function(sName)
	{
		var aCookie = document.cookie.split("; ");
		for (var i=0; i < aCookie.length; i++)
		{
			var aCrumb = aCookie[i].split("=");
			if (sName == aCrumb[0])
			{
				return aCrumb.length>=2 ? (aCrumb[1]) : "";
			}
		}
		return "";
	}
}