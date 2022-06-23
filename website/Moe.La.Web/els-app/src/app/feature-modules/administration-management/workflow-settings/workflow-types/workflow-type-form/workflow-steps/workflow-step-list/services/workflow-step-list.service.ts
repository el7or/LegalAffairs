import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { Constants } from 'app/core/constants';
import { DataService } from 'app/core/services/data.service';

@Injectable({ providedIn: 'root' })
export class WorkflowStepListService extends DataService {
    constructor(http: HttpClient) {
        super(Constants.WORKFLOW_LOOKUPS_API_URL, http);
    }

    /**
     * Get all workflow steps.
     */
    GetAllWorkflowSteps() {
        return this.http.get(this.url + "/workflow-steps");
    }


    /**
     * Get workflow steps for a given workflow type.
     * @param workflowTypeId workflow type ID.
     */
    GetWorkflowStepsByWorkflowTypeId(workflowTypeId: string) {
        return this.http.get(`${this.url}/workflow-types/${workflowTypeId}/steps`);
    }
}
