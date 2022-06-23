import { Guid } from "guid-typescript";

import { BaseModel } from "./base-model";

/**
 * Used to create workflow step objects.
 */
export class WorkflowStep extends BaseModel<string> {
    constructor() {
        super();
        this.id = Guid.createEmpty().toString();
        this.createdBy = Guid.createEmpty().toString();
    }

    /**
     * The workflow type related to.
     */
    workflowTypeId: string = '';

    /**
     * The workflow step category belongs to.
     */
    workflowStepCategoryId: number = 0;

    /**
     * The workflow step Arabic name.
     */
    stepArName: string = '';

    roles: string[] = [];
}

/**
 * Used to create workflow step list item objects.
 */
export class WorkflowStepListItem {
    /**
     * ID
     */
    id: string = '';

    /**
     * The workflow type related to.
     */
    workflowTypeId: string = '';

    /**
     * The workflow type Arabic name.
     */
    workflowTypeArName: string = '';

    /**
     * The workflow step category belongs to.
     */
    workflowStepCategoryId: number = 0;

    /**
     * The workflow step category Arabic name.
     */
    workflowStepCategoryArName: string = '';

    /**
     * The workflow step Arabic name.
     */
    stepArName: string = '';

    roles: string[] = [];
}
