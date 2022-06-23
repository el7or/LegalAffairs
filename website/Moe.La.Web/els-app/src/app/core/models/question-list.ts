import { KeyValuePairs } from "./key-value-pairs";

export class QuestionList {
  id: number = 0;
  question: string = '';
  status:KeyValuePairs<number>={id:0,name:''}
  answer: string = '';
}