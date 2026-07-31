import { Message } from "./messaging.ts"

export enum MessageResult {
    SUCCESS,
    FINISHED,
    ERROR
}

export interface MessageHandler {
    HandleMessage(message: Message): MessageResult;
}
