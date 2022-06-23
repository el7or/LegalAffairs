import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { Constants } from 'app/core/constants';
import { DataService } from 'app/core/services/data.service';
import { WorkflowStep } from 'app/core/models/workflow-step'

@Injectable({ providedIn: 'root' })
export class WorkflowStepService extends DataService {
    constructor(http: HttpClient) {
        super(Constants.WORKFLOW_CONFIGURATION_API_URL, http);
    }

    /**
     * Create new workflow step.
     * @param workflowStep The workflow type object to be created.
     */
    createWorkflowStep(workflowStep: WorkflowStep) {

        return this.http.post(`${this.url}/add-workflow-step`, workflowStep);
    }

    /**
     * Get workflow step categories.
     */
    getWorkflowStepCategories() {
        return this.http.get(`${Constants.WORKFLOW_LOOKUPS_API_URL}/workflow-step-categories`);
    }

    /**
     * Get all workflow types.
     */
    GetWorkflowTypes() {
        return this.http.get(`${Constants.WORKFLOW_LOOKUPS_API_URL}/workflow-types`);
    }

    /**
     * Update workflow step.
     * @param workflowStep Workflow step object to be updated.
     */
    updateWorkflowStep(workflowStep: WorkflowStep) {
        return this.http.post(`${this.url}/update-workflow-step`, workflowStep);
    }

    /**
     * Get workflow step.
     * @param id Workflow step ID.
     */
    getWorkflowStep(id: string) {
        return this.http.get(`${Constants.WORKFLOW_LOOKUPS_API_URL}/workflow-steps/${id}`);
    }
}
