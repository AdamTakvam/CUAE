document.write('<script type="text/javascript" src="/appsuiteadmin/jpspan_user_autocomplete.php?client"></script>');

function selectName(FormName, FieldName, value)
{
    document.forms[FormName].elements[FieldName].value = value;
    closeFancyDropdown()
}

function closeFancyDropdown()
{
    document.getElementById("fancyDropdown").style['visibility'] = "hidden";
}


function getUser(input, evt)
{
    getUser(input, evt, 0);
}


function getUser(input, evt, group_limit) {
    
    if (input.value.length == 0) {
        closeFancyDropdown();
        return;
    }

    // Ignore the following keystrokes
    switch (evt.keyCode) {
        case 37: //left arrow
        case 39: //right arrow
        case 33: //page up  
        case 34: //page down  
        case 36: //home  
        case 35: //end
        case 13: //enter
        case 9: //tab
        case 27: //esc
        case 16: //shift  
        case 17: //ctrl  
        case 18: //alt  
        case 20: //caps lock
        case 38: //up arrow
        case 40: //down arrow
            return;
        break;
    }
    
    // Create the remote client
    var a = new userautocomplete(acCallback);
    
    // Set a timeout for responses which take too long
    a.timeout = 3000;
    
    // Ignore timeouts
    a.clientErrorFunc = function(e) {
        if ( e.code == 1003 ) {
            // Ignore...
        } else {
            alert(e);
        }
    }
    
    // Call the remote method
    a.getuser(input.value, group_limit);
}

var acCallback = {

    field: '',
    formname: '',

    getuser: function(result)
    {
        r_length = result.length
        if (r_length < 1 ) {
            closeFancyDropdown()
            return;
        }
        else
        {
            list = ''
            for (x = 0; x < r_length; x++)
            {
                list += '<li><a href="#" onclick="javscript:selectName(\'' + this.formname.name + '\',\'' + this.field.name + '\',\'' + result[x] + '\')">' + result[x] + '</a></li>';
            }
            dropdown = document.getElementById('fancyDropdown')
            dropdown.innerHTML = list;
            dropdown.style['visibility'] = "visible";            

            if ((navigator.userAgent.toLowerCase().indexOf('msie') + 1) > 0)
            {
                // http://homepage.mac.com/igstudio/design/ulsmenus/vertical-uls-iframe.html
                // Fix for dropdown display over <select> elements in IE
                /** IE script to cover <select> elements with <iframe>s **/
                dropdown.innerHTML = ('<iframe src="about:blank" scrolling="no" frameborder="0"></iframe>' + dropdown.innerHTML);
                var ieMat = dropdown.firstChild;
                        ieMat.style.width=dropdown.offsetWidth+"px";
                        ieMat.style.height=dropdown.offsetHeight+"px";	
                        dropdown.style.zIndex="99";
                /** IE script to change class on mouseover **/
                var ieLIs = dropdown.getElementsByTagName('li');
                for (var i=0; i<ieLIs.length; i++) if (ieLIs[i]) {
                    ieLIs[i].onmouseover=function() {this.className+=" iehover";}
                    ieLIs[i].onmouseout=function() {this.className=this.className.replace(' iehover', '');}
                }
            }

            return;
        }
    }
}
