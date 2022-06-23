import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { Constants } from 'app/core/constants';
import { DataService } from 'app/core/services/data.service';
import { WorkflowAction } from 'app/core/models/workflow-action'

@Injectable({ providedIn: 'root' })
export class WorkflowActionService extends DataService {
    constructor(http: HttpClient) {
        super(Constants.WORKFLOW_CONFIGURATION_API_URL, http);
    }

    /**
     * Create new workflow action.
     * @param workflowAction Workflow action object to be created.
     */
    createWorkflowAction(workflowAction: WorkflowAction) {
        return this.http.post(`${this.url}/add-workflow-action`, workflowAction);
    }

    /**
     * Update workflow action.
     * @param workflowAction Workflow action object to be updated.
     */
    updateWorkflowAction(workflowAction: WorkflowAction) {

        return this.http.post(`${this.url}/update-workflow-action`, workflowAction);
    }

    /**
     * Get workflow action.
     * @param id Workflow action id.
     */
    getWorkflowAction(id: string) {
        return this.http.get(`${Constants.WORKFLOW_LOOKUPS_API_URL}/workflow-actions/${id}`);
    }
}
