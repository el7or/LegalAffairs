/**
 * Used to create events.
 */
export class EmitEvent {
    constructor(public name: any, public value?: any) { }
}

/**
 * The workflow type events reflected by the UI operations on the workflow type.
 */
export enum WorkflowTypeEvents {
    // When a component emit the workflow type ID.
    WorkflowTypeEmitted
}