export interface IMeetingNotes {
    Subject?: string,
    Date?: Date,
    Secretary?: string,
    Participants?: IParticipant[],
    Decisions?: IDecision[]
}

export interface IParticipant {
    Name?: string
}

export interface IDecision {
    Problem?: string,
    Solution?: string,
    Responsible?: string,
    ControlDate?: Date
}