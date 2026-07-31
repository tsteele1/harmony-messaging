import type { Message } from "./messaging"

export enum MessageResult {
    SUCCESS,
    FINISHED,
    ERROR
}

export interface MessageHandler {
    HandleMessage(message: Message): MessageResult;
}
