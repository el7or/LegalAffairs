import { Guid } from "guid-typescript";

import { BaseModel } from "./base-model";

/**
 * Used to create workflow type objects.
 */
export class WorkflowAction extends BaseModel<string> {
    constructor() {
        super();
        this.id = Guid.createEmpty().toString();
        this.createdBy = Guid.createEmpty().toString();
    }

    /**
     * The workflow type this action related to.
     */
    workflowTypeId: string = '';

    /**
     * The workflow action English name.
     */
    actionArName: string = '';
}

/**
 * Used to create workflow action list item objects.
 */
export class WorkflowActionListItem {
    /**
     * ID
     */
    id: string = '';

    /**
     * The workflow type this action related to.
     */
    workflowTypeId: string = '';

    /**
     * Workflow type Arabic name.
     */
    workflowTypeArName: string = '';

    /**
     * The workflow action Arabic name.
     */
    actionArName: string = '';
}

// /**
//  * Used to create workflow action view object.
//  */
// export class WorkflowActionView {
//     /**
//     * ID
//     */
//     Id: string = '';

//     /**
//     * The workflow type this action related to.
//     */
//     workflowTypeId: string = '';

//     /**
//      * The workflow action Arabic name.
//      */
//     actionArName: string = '';
// }