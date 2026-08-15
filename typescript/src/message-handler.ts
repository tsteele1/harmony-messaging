import type { Message } from "./messaging"

export interface MessageHandler {
    HandleMessage(message: Message): void;
}
