import React, { Component, ReactNode } from "react";
import { IMeetingNotes, IParticipant, IDecision } from "./IMeetingNotes";
import { Text, Fabric, Stack, TextField, DatePicker, DayOfWeek, Label, Button, DetailsList } from "office-ui-fabric-react";

export interface IMeetingNotesState {
    Subject?: string,
    MeetingDate?: Date,
    Secretary?: string,
    Participants: IParticipant[],
    Decisions: IDecision[]
}

export interface IMeetingNotesProps {
    meetingNotes: IMeetingNotes;
}

export class MeetingNotes extends Component<IMeetingNotesProps, IMeetingNotesState> {
    constructor(props: IMeetingNotesProps) {
        super(props);
        const { Subject, Date, Decisions = [], Participants = [], Secretary } = this.props.meetingNotes;
        this.state = { Subject, MeetingDate: Date, Decisions, Participants, Secretary };
    }

    render(): ReactNode {

        const { Subject, MeetingDate, Decisions, Participants, Secretary } = this.state;

        return (
            <Fabric style={{ width: 800 }}>
                <Text variant="xLarge">Word Form generation sample</Text>

                <fieldset>
                    <legend>Meeting Notes</legend>
                    <Stack tokens={{ childrenGap: 5 }}>
                        <TextField label="Subject" value={Subject} onChange={(e, v) => this.setState({ Subject: v })} />
                        <DatePicker allowTextInput={true} firstDayOfWeek={DayOfWeek.Monday} label="Date" value={MeetingDate}
                            onSelectDate={(d) => this._setDate(d)} />
                        <TextField label="Secretary" value={Secretary} onChange={(e, v) => this.setState({ Secretary: v })} />
                        <Label>Participants</Label>
                        <Stack horizontal horizontalAlign="end">
                            <Button onClick={() => this._addParticipant()}>+ Add new participant</Button>
                        </Stack>
                        <DetailsList items={Participants}></DetailsList>
                        <Label>Decisions</Label>
                        <Stack horizontal horizontalAlign="end">
                            <Button onClick={() => this._addDecision()}>+ Add new decision</Button>
                        </Stack>
                        <DetailsList items={Decisions}></DetailsList>
                        <Stack horizontal tokens={{ childrenGap: 10 }}>
                            <Button onClick={(e) => this._Generate()}>Generate</Button>
                            <Button onClick={(e) => this._clearForm()}>Clear form</Button>
                        </Stack>
                    </Stack>
                </fieldset>
            </Fabric>
        );
    }

    _Generate(): void {
        const meetingNotes: IMeetingNotes = { 
            Secretary: this.state.Secretary,
            Subject: this.state.Subject,
            Date: this.state.MeetingDate,
            Decisions: this.state.Decisions,
            Participants: this.state.Participants
        };

        console.log(meetingNotes);
    }

    _clearForm(): void {
        this.setState({ Subject: "", Secretary: "", Participants: [], Decisions: [], MeetingDate: undefined });
    }

    _addParticipant(): void {
        const newParticipant: IParticipant = { Name: "Some name" };
        this.setState({ Participants: this.state.Participants.concat([newParticipant]) });
    }

    _setDate(d?: Date | null): void {
        if (!d)
            d = undefined;
        this.setState({ MeetingDate: d });
    }

    _addDecision() {
        const newDecision: IDecision = { Problem: "Some prob", Solution: "Sol", Responsible: "Res" };
        this.setState({ Decisions: this.state.Decisions.concat([newDecision]) });
    }
};
