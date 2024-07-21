import { ITemplate, AbstractPageInitializer } from "./../../interfaces/irootbundle"
import "parsleyjs";

class LoginPageInitializer extends AbstractPageInitializer implements ITemplate {
  onStart(): void {
    $(".login_content input").val('');
    $("#loginPage").removeClass("hidden");

    var self = this;
    $('#logInBtn').on('click.loginBtnEvent', self.loginBtnEvent.bind(self));
  }

  dispose(): void {
    $("#loginPage").addClass("hidden");
    $("#logInBtn").off("click.loginBtnEvent");
  }

  private loginBtnEvent() {
    var self = this;

    var loginFormContainer = $('#loginPage form');
    var loginFormValidation = loginFormContainer.parsley();
    if (loginFormValidation.isValid()) {
      var username = loginFormContainer.find('input[name="username"]').val();
      var password = loginFormContainer.find('input[name="password"]').val();

      $.ajax("/Account/Login", {
        method: "POST",
        contentType: "application/json",
        dataType: "json",
        data: JSON.stringify({
          UserName: username,
          Password: password,
          RememberMe: false
        })
      }).done(self._disposableListenerWrapper.bind(self, function (data) {
        $("#errorMessage").addClass("hidden");

        var val = self._getDecodedParameter("ReturnUrl");
        if (val != null) {
          location.href = location.origin + val;
        }

      })).catch(self._disposableListenerWrapper.bind(self, function (err) {
        if (err.status == 401) {
          $("#errorMessage").removeClass("hidden");
        } else {
          console.error(err);
        }
      }));
    } else {
      loginFormValidation.validate();
    };

    return false;
  }
}

export { LoginPageInitializer }