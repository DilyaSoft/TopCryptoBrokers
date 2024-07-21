declare namespace parsley {

  type RequirementTypes = 'string' | 'integer' | 'number' | 'regexp' | 'boolean'
  type ValidatorReturn = boolean | JQueryPromise<boolean>;

  interface ValidatorOptions {
    requirementType?: RequirementTypes
    validateString?: (value: string, requirement: string, field?: Field) => ValidatorReturn;
    validateNumber?: (value: number, requirement: number, field?: Field) => ValidatorReturn;
    validateMultiple?: (value: Array<any>, requirement: any, field?: Field) => ValidatorReturn;
    messages?: { [locale: string]: string }
    priority: number;
  }


  interface ValidationOptions {
    group: string;
    force: boolean;
  }

  interface GeneralOptions {
    /**
     * Default data namespace for DOM API.
     * @default 'data-parsley-'
     */
    namespace: string;
    /**
     * Comma separated selectors for targeted elements.
     * @default 'input, textarea, select'
     */
    inputs: string;
    /**
     * Comma separated selectors for elements excluded from the {@link inputs}.
     * @default 'input[type=button], input[type=submit], input[type=reset], input[type=hidden]'
     */
    excluded: string;
    /**
     * Stops validating field on highest priority failing constraint.
     * @default true
     */
    priorityEnabled: boolean;
  }

  interface FieldOnlyOptions {
    multiple?: string;
    group?: string;
    uiEnabled?: boolean;

  }

  interface FieldCoreOptions extends FieldOnlyOptions {
    value?: (parsley: Field) => any;
    errorMessage?: string;
  }

  interface UIOptions {
    validationThreshold?: number;
    focus?: 'first' | 'last' | 'none';
    trigger?: boolean | string;
    triggerAfterFailure?: boolean | string;
    errorClass?: string;
    successClass?: string;
    classHandler?: (field: Field) => JQuery | string;
    errorsContainer?: (field: Field) => JQuery | string;
    errorsWrapper?: string | JQuery;
    errorTemplate?: string | JQuery;
  }

  interface DefaultOptions extends GeneralOptions, UIOptions {

  }

  interface FieldOptions extends FieldCoreOptions, UIOptions {

  }

  interface FormOptions extends UIOptions, GeneralOptions {

  }

  interface Events {
    /**
     * Attaches an event handler to an event.
     * @param eventName Name of the event.
     * @param handler Event handler.
     */
    on(eventName: string, handler: (event: any) => void);
    /**
     * Detaches an event handler from an event.
     * @param eventName Name of the event.
     * @param handler Event handler.
     */
    off(eventName: string, handler: (event: any) => void);
    /**
     * Attaches an event handler to an event.
     * @param eventName Name of the event.
     * @param handler Event handler.
     * @deprecated Use {@link on} instead.
     */
    subscribe(eventName: string, handler: (event: any) => void);
    /**
     * Detaches an event handler from an event.
     * @param eventName Name of the event.
     * @param handler Event handler to remove.
     * @deprecated Use {@link off} instead.
     */
    unsubscribe(eventName: string, handler: (event: any) => void);
    /**
     * Triggers an event. Return value of false interrupts the callback chain.
     * @param eventName Event to be triggered.
     * @param target Target element. Uses context otherwise.
     * @param extraArg Additional arguments to be provided to the event.
     */
    trigger(eventName: string, target?, extraArg?: any): boolean;
  }

  interface FieldUI {
    /**
     * @return Array of current error messages for this field. Does not include error messages added/updated through {@link addError} and {@link updateError}.
     */
    getErrorsMessages(): Array<string>;
    addError(name: string, options: { message: string, assert: any, updateClass: boolean });
    updateError(name: string, options: { message: string, assert: any, updateClass: boolean });
    removeError(name: string, options: { updateClass: boolean });
  }

  interface FormUI {
    focus(): JQuery;
  }

  interface MultipleField {
    addElement($element: JQuery): Field;
  }

  interface Form extends Abstract, FormUI {
    isValid(options?: ValidationOptions): boolean | null;
    validate(options?: ValidationOptions): boolean | null;
    whenValid(options?: ValidationOptions): JQueryPromise<any>;
    whenValidate(options?: ValidationOptions): JQueryPromise<any>;
    /**
     * Resets the UI for this form and its fields.
     */
    reset(): void;
    /**
     * Disable and destroy Parsley for this form.
     */
    destroy(): void;
    submitEvent: Event;
    options: ValidationOptions;
    $element: JQuery;
  }

  interface Factory extends Abstract {

  }

  interface Field extends Abstract, FieldUI {
    /**
     * Resets the UI for this field.
     */
    reset(): void;
    /**
     * Disable and destroy Parsley for this field.
     */
    destroy(): void;
    $element: JQuery;
    isValid(options?: { force?: boolean }): boolean | null;
    validate(options?: ValidationOptions): boolean | null;

    hasConstraints(): boolean;
    refreshConstraints(): void;
    addConstraint(name: string, requirements?: any, priority?: number, isDomConstraint?: boolean): Field;
    removeConstraint(name: string): Field;
    updateConstraint(name: string, requirements: any, priority: number): Field;
    needsValidation(): boolean;
    getValue(): string;
    options: FieldOptions;

  }

  interface Utils {
    attr($element, namespace, obj);
    checkAttr($element, namespace, checkAttr): boolean;
    setAttr($element, namespace, attr, value);
    generateID(): string;
    deserializeValue(value);
    camelize(value: string): string;
    dasherize(value: string): string;
    warn(...args: any[])
    warnOnce(message: string, ...args: any[])
    trimString(value: string): string;
    namespaceEvents(events: string, namespace: string): string;
    difference<T>(array: T[], remove: T[]): T[];
    all<T>(promises: JQueryPromise<T>[]): JQueryPromise<T>;
  }

  interface Abstract extends Events {
    actualizeOptions();
    /**
     * Resets the UI.
     */
    reset();
    /**
     * Removes Parsley from this field or form.
     */
    destroy();
  }

  interface ValidatorRegistry {
    setLocate(locale: string): ValidatorRegistry;
    addCatalog(locale: string, messages, set: boolean): ValidatorRegistry
    addMessage(locale: string, name: string, message: string): ValidatorRegistry;
    addMessages(locale: string, nameMessageObject: { [name: string]: string })
    addValidator(name: string, validator, priority: number)
    getErrorMessage(constraint): string;
    formatMessage(string, parameters): string;
    updateValidator();
    removeValidator();
  }

  class Validator {
    constructor(spec: ValidatorOptions);
    validate(value, ...args): ValidatorReturn;
    parseRequirements(requirements, extraOptionReader);
  }

  interface Static extends ValidatorRegistry {
    addAsyncValidator(validatorName: string, xhrFunction, url: string)
    addValidator(validatorName: string, options: ValidatorOptions)
    options: DefaultOptions;
    UI: UIStatic;
  }

  /**
   * @deprecated Accessing {@link parsley.UIStatic} methods is deprecated. Call methods directly on {@link parsley.Field} instead.
   */
  interface UIStatic {
    removeError(instance, name, doNotUpdateClass);
    getErrorsMessages(instance);
    addError(instance, name, message, assert, doNotUpdateClass);
    updateError(instance, name, message, assert, doNotUpdateClass);
  }

}
/**
 * Parsley extends JQuery objects with the following methods.
 */
interface JQuery {
  parsley(options?: parsley.FieldOptions): parsley.Field;
  parsley(options?: parsley.FormOptions): parsley.Form;
}

/**
 * Make Window properties available.
 */
interface Window {
  Parsley: parsley.Static;
  psly: parsley.Static;
  ParsleyUtils: parsley.Utils;
  /**
   * @deprecated access options through window.Parsley.options instead.
   */
  ParsleyConfig: parsley.DefaultOptions;
  /**
   * @deprecated Accessing ParsleyValidator is deprecated. Call on window.Parsley instead.
   */
  ParsleyValidator: parsley.ValidatorRegistry;
  /**
   * @deprecated Accessing {@link parsley.UIStatic} methods is deprecated. Call methods directly on {@link parsley.Field} instead.
   */
  ParsleyUI: parsley.UIStatic
}