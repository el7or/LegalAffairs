import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { filter, map } from 'rxjs/operators';

import { EmitEvent, WorkflowTypeEvents } from 'app/core/enums/workflow/workflow-type-events';
import { WorkflowTypeView } from 'app/core/models/workflow-type';

@Injectable({ providedIn: 'root' })
export class WorkflowTypeFormService {
    private subject = new BehaviorSubject<any>(new EmitEvent(''));
    private _workflowTypeView: WorkflowTypeView = new WorkflowTypeView();

    constructor() {
    }

    public on(event: WorkflowTypeEvents, action: any) {
        return this.subject.pipe(
            filter((e: EmitEvent) => { return e.name === event; }),
            map((e: EmitEvent) => { return e.value; }))
            .subscribe(action);
    }

    public emit(event: EmitEvent) {
        return this.subject.next(event);
    }

    get workflowTypeView(): WorkflowTypeView {
        return this._workflowTypeView;
    }

    set workflowTypeView(value: WorkflowTypeView) {
        this._workflowTypeView = value;
    }

    /**
     * Reset data holded by the service.
     */
    public reset(): void {
        this.workflowTypeView = new WorkflowTypeView();
    }
}
