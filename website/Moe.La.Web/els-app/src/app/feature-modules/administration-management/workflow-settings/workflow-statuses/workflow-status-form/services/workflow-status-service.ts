import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { Constants } from 'app/core/constants';
import { DataService } from 'app/core/services/data.service';
import { WorkflowStatus } from 'app/core/models/workflow-status'

@Injectable({ providedIn: 'root' })
export class WorkflowStatusService extends DataService {
    constructor(http: HttpClient) {
        super(Constants.WORKFLOW_CONFIGURATION_API_URL, http);
    }

    /**
     * Create new workflow status.
     * @param workflowStatus The workflow status object to be created.
     */
    createWorkflowStatus(workflowStatus: WorkflowStatus) {

        return this.http.post(this.url + "/add-workflow-status", workflowStatus);
    }
}
