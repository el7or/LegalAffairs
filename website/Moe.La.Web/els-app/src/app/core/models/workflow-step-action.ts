import { Guid } from "guid-typescript";

import { BaseModel } from "./base-model";

export class WorkflowStepAction extends BaseModel<string> {
    constructor() {
        super();
        this.id = Guid.createEmpty().toString();
        this.createdBy = Guid.createEmpty().toString();
    }

    workflowStepId: string = '';

    workflowActionId: string = '';

    nextStepId: string = '';

    nextStatusId: number = 0;

    description: string = '';

    isNoteVisible: boolean = false;

    isNoteRequired: boolean = false;
}

export class WorkflowStepActionListItem extends BaseModel<string> {
    id: string = '';

    workflowStepId: string = '';

    workflowStepName: string = '';

    workflowActionId: string = '';

    workflowActionName: string = '';

    nextStepId: string = '';

    nextStepName: string = '';

    nextStatusId: number = 0;

    nextStatusName: string = '';

    description: string = '';

    isNoteVisible: boolean = false;

    isNoteRequired: boolean = false;
}