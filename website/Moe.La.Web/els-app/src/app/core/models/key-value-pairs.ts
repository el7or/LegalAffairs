
export class KeyValuePairs<T=number> {
  id: T | undefined;
  name: string = '';
  public constructor(init?: Partial<KeyValuePairs<T>>) {
    Object.assign(this, init);
  }
}


