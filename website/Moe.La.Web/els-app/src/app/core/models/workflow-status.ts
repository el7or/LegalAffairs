import { Guid } from "guid-typescript";

import { BaseModel } from "./base-model";

/**
 * Used to create workflow status objects.
 */
export class WorkflowStatus extends BaseModel<number> {

    constructor() {
        super();
        this.createdBy = Guid.createEmpty().toString();
    }

    /**
     * The workflow status Arabic name.
     */
    statusArName: string = '';
}

/**
 * Used to create workflow status list item objects.
 */
export class WorkflowStatusListItem {
    /**
     * ID
     */
    id: string = '';

    /**
     * The workflow status Arabic name.
     */
    statusArName: string = '';
}

/**
 * Used to create workflow status object.
 */
export class WorkflowStatusView {
    /**
     * ID
     */
    id: string = '';

    /**
     * The workflow status Arabic name.
     */
    statusArName: string = '';
}