import { Injectable } from "@angular/core";
import { AbstractControl, ValidationErrors, Validator } from "@angular/forms";

@Injectable({ providedIn: "root" })
export class MatchPassword implements Validator {
    validate(control: AbstractControl): ValidationErrors {
        const { password, passwordConfirmation } = control.value;

        if (password === passwordConfirmation) {
            return null;
        } else {
            return { passwordsDontMatch: true };
        }
    }
}