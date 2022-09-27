function changeColor() {
    var x = document.getElementById("phoneNumber").value;
    var y = document.getElementById("codeOrder").value;

    if (x == "" && y == "") 
    {
      document.getElementById("phoneNumber").style.border = "1px solid red";
      document.getElementById("codeOrder").style.border = "1px solid red";
      document.getElementById("error").style.display = "block";
    }

    function normalColor() {
        document.getElementById("phoneNumber").style.border = "";
        document.getElementById("codeOrder").style.border = "";
        document.getElementById("error").style.display = "";
    }
    
    setTimeout(normalColor, 2000);
}
