﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BugReport.DataModel
{
    public class DataModelIssue
    {
        public int Id;
        public int Number;
        public string Title;
        public Octokit.ItemState State;
        public User Assignee;
        public Label[] Labels;
        public User User;
        public string HtmlUrl;

        public DateTimeOffset? CreatedAt;
        public DateTimeOffset? UpdatedAt;
        public DateTimeOffset? ClosedAt;
        public User ClosedBy;

        public PullRequest PullRequest;
        public Milestone Milestone;

        public bool IsIssueOrComment
        {
            get { return (PullRequest == null); }
        }
        public bool IsPullRequest
        {
            get { return (PullRequest != null); }
        }
        public bool IsIssueKind(IssueKindFlags flags)
        {
            if (IsIssueOrComment)
            {
                if (HtmlUrl.Contains("issuecomment"))
                    return ((flags & IssueKindFlags.Comment) != 0);
                else
                    return ((flags & IssueKindFlags.Issue) != 0);
            }
            // IsPullRequest
            return ((flags & IssueKindFlags.PullRequest) != 0);
        }

        public bool IsOpen
        {
            get { return (State == Octokit.ItemState.Open); }
        }
        public bool IsClosed
        {
            get { return (State == Octokit.ItemState.Closed); }
        }

        public bool HasLabel(string labelName)
        {
            return Labels.ContainsLabel(labelName);
        }

        public bool IsMilestone(string milestoneName)
        {
            if (Milestone == null)
            {
                return (milestoneName == null);
            }
            return (Milestone.Title == milestoneName);
        }

        public bool HasAssignee(string assigneeName)
        {
            if (Assignee == null)
            {
                return (assigneeName == null);
            }
            return (Assignee.Name == assigneeName);
        }

        public override string ToString()
        {
            StringWriter sw = new StringWriter();
            sw.WriteLine("Number: {0}", Number);
            sw.WriteLine("Type: {0}", (PullRequest == null) ? "Issue" : "PullRequest");
            sw.WriteLine("URL: {0}", HtmlUrl);
            sw.WriteLine("State: {0}", State);
            sw.WriteLine("Assignee.Name:  {0}", (Assignee == null) ? "<null>" : Assignee.Name);
            sw.WriteLine("        .Login: {0}", (Assignee == null) ? "<null>" : Assignee.Login);
            sw.WriteLine("Labels.Name:");
            foreach (Label label in Labels)
            {
                sw.WriteLine("    {0}", label.Name);
            }
            sw.WriteLine("Title: {0}", Title);
            //sw.WriteLine("Milestone.Title: {0}", (issue.Milestone == null) ? "<null>" : issue.Milestone.Title);
            sw.WriteLine("User.Name:  {0}", (User == null) ? "<null>" : User.Name);
            sw.WriteLine("    .Login: {0}", (User == null) ? "<null>" : User.Login);
            sw.WriteLine("CreatedAt: {0}", CreatedAt);
            sw.WriteLine("UpdatedAt: {0}", UpdatedAt);
            sw.WriteLine("ClosedAt:  {0}", ClosedAt);
            sw.WriteLine("ClosedBy.Name:  {0}", (ClosedBy == null) ? "<null>" : ClosedBy.Name);
            sw.Write("        .Login: {0}", (ClosedBy == null) ? "<null>" : ClosedBy.Login);

            string text = sw.ToString();
            sw.Close();
            return text;
        }
    }

    [FlagsAttribute]
    public enum IssueKindFlags
    {
        Issue = 1,
        PullRequest = 2,
        Comment = 4,
        All = Issue | PullRequest | Comment
    }

    public class Label
    {
        public string Name;

        public Label(string name)
        {
            Name = name;
        }
    }

    public class User
    {
        public string Name;
        public string Login;
        public string HtmlUrl;
    }

    public class Milestone
    {
        public int Number;
        public string Title;
        public string Description;
        public int OpenIssues;
        public int ClosedIssues;
        public Octokit.ItemState State;
        public User Creator;
        public DateTimeOffset CreatedAt;
        public DateTimeOffset? DueOn;
        public DateTimeOffset? ClosedAt;
    }

    public class PullRequest
    {
        //public string HtmlUrl;

        /*
        public DateTimeOffset? CreatedAt;
        public DateTimeOffset? UpdatedAt;
        public DateTimeOffset? ClosedAt;
        public DateTimeOffset? MergedAt;
        public bool Merged;
        */
    }
}