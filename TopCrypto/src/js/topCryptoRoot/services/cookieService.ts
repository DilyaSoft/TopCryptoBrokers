class CookieService {
  private _ASP_CORE_CULTURE_NAME = ".AspNetCore.Culture";

  setCookie(cname, cvalue, minutes) {
    var d = new Date();
    d.setTime(d.getTime() + (minutes * 60 * 1000));
    var expires = "expires=" + d.toUTCString();
    document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
  }

  setUserInterfaceCulture(val) {
    var cookieVal = "c=" + val + "|uic=" + val;
    this.setCookie(this._ASP_CORE_CULTURE_NAME, cookieVal, 60);
  }

  getCookie(cname) {
    var name = cname + "=";
    var decodedCookie = decodeURIComponent(document.cookie);
    var ca = decodedCookie.split(';');
    for (var i = 0; i < ca.length; i++) {
      var c = ca[i];
      while (c.charAt(0) == ' ') {
        c = c.substring(1);
      }
      if (c.indexOf(name) == 0) {
        return c.substring(name.length, c.length);
      }
    }
    return "";
  }

  getUserInterfaceCulture() {
    var val = this.getCookie(this._ASP_CORE_CULTURE_NAME);
    if (!val) return null;
    return val.split("|")[1].substring(4);
  }
}

export { CookieService };