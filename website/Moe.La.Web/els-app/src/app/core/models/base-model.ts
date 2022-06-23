/**
 * The base model class.
 */
export class BaseModel<T> {
    /**
     * The primary key.
     */
    id: T | undefined;

    /**
     * The user's id who created it.
     */
    createdBy: string = '';

    /**
     * The creation datetime.
     */
    createdOn: Date = new Date();

    /**
     * The user's id who updated it.
     */
    updatedBy: string = '';

    /**
     * The update datetime.
     */
    updatedOn?: Date;

    /**
     * Logical delete, if true don't show it.
     */
    isDeleted: boolean = false;
}