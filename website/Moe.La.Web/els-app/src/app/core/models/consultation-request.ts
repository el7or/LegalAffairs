import { Request } from './supporting-document-request';

export class ConsultationSupportingDocument {

    consultationId: number;

    moamalaId: number;

    requestId: number;
    
    request: Request = new Request();

    public constructor(init?: Partial<ConsultationSupportingDocument>) {
        Object.assign(this, init);
    }
}