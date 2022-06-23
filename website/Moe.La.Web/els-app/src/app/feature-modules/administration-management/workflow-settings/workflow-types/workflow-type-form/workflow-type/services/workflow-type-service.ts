import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { Constants } from 'app/core/constants';
import { DataService } from 'app/core/services/data.service';
import { WorkflowType, WorkflowTypeView } from 'app/core/models/workflow-type'
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class WorkflowTypeService extends DataService {
    constructor(http: HttpClient) {
        super(Constants.WORKFLOW_CONFIGURATION_API_URL, http);
    }

    /**
     * Create new workflow type.
     * @param workflowType The workflow type object to be created.
     */
    createWorkflowType(workflowType: WorkflowType) {
        return this.http.post<WorkflowType>(`${this.url}/add-workflow-type`, workflowType);
    }

    /**
     * Update workflow type.
     * @param workflowType The workflow type object to be updated.
     */
    updateWorkflowType(workflowType: WorkflowType) {
        return this.http.post<WorkflowType>(`${this.url}/update-workflow-type`, workflowType);
    }

    /**
     * Get full workflow type information.
     * @param id workflow type ID..
     */
    getWorkflowTypeView(id: string) {
        return this.http.get<WorkflowTypeView>(`${Constants.WORKFLOW_LOOKUPS_API_URL}/workflow-types/${id}`);
    }
}
