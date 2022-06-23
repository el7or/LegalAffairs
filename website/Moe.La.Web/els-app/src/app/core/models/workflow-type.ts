import { Guid } from "guid-typescript";

import { BaseModel } from "./base-model";
import { WorkflowActionListItem } from './workflow-action';
import { WorkflowStepListItem } from './workflow-step';
import { WorkflowStepActionListItem } from './workflow-step-action';

/**
 * Used to create workflow type objects.
 */
export class WorkflowType extends BaseModel<string> {
    constructor() {
        super();
        this.id = Guid.createEmpty().toString();
        this.createdBy = Guid.createEmpty().toString();
    }

    /**
     * The workflow type Arabic name.
     */
    typeArName: string = '';

    /**
     * Detemines wether the workflow type is active or not.
     */
    isActive: boolean = false;

    /**
     * Detemines the amount of time allowed for this workflow type to be locked by a given user.
     */
    lockPeriod: number = 0; // hours.
}

/**
 * Used to create workflow type list item objects.
 */
export class WorkflowTypeListItem {
    /**
     * ID
     */
    id: string = '';

    /**
     * The workflow type Arabic name.
     */
    typeArName: string = '';

    /**
     * Detemines wether the workflow type is active or not.
     */
    isActive: boolean = false;

    /**
     * Detemines the amount of time allowed for this workflow type to be locked by a given user.
     */
    lockPeriod: number = 0; // hours.
}

/**
 * A view model for workflow type component.
 */
export class WorkflowTypeView {
    /**
     * Workflow type.
     */
    workflowType: WorkflowType = null!;

    /**
     * Workflow actions.
     */
    workflowActions: WorkflowActionListItem[] = [];

    /**
     * Workflow steps.
     */
    workflowSteps: WorkflowStepListItem[] = [];

    /**
     * Workflow steps actions.
     */
    workflowStepsActions: WorkflowStepActionListItem[] = [];
}
